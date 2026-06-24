using Logistics.Application.DTOs.Requests;
using Logistics.Application.DTOs.Responses;
using Logistics.Application.Services;
using Logistics.Domain.Entities;
using Logistics.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Services;

/// <summary>
/// Lógica de control de acceso: genera los QR de prueba y valida los QR
/// escaneados registrando el consumo de raciones contra la base de datos.
/// </summary>
public class EventAccessService(
    LogisticsDbContext dbContext,
    QrGenerationService qrGenerationService,
    MailpitIntegrationService mailService,
    IConfirmedParticipantsProvider confirmedParticipantsProvider) : IEventAccessService
{
    public async Task SimulateEventStartAsync(SimulateEventStartRequest request)
    {
        foreach (var participantId in request.ParticipantIds)
        {
            var payload = qrGenerationService.GenerateParticipantQrPayload(participantId, request.EventId);
            var qrImageBytes = qrGenerationService.GenerateQrImage(payload);
            await mailService.SendQrEmailAsync(participantId, qrImageBytes);
        }
    }

    public async Task<QrValidationResponse> ValidateQrAsync(
        ValidateQrRequest request,
        CancellationToken cancellationToken = default)
    {
        // 1. Parsear el payload "{eventId}:{participantId}".
        if (!TryParsePayload(request.Payload, out var eventId, out var participantId))
        {
            return Fail("QR inválido o vacío.");
        }

        // 2. Verificar que el participante pertenezca a una orden confirmada del evento.
        var participant = await confirmedParticipantsProvider
            .FindConfirmedAsync(eventId, participantId, cancellationToken);

        if (participant is null)
        {
            return Fail("Participante no confirmado para este evento.");
        }

        // 3. Resolver (o crear) la configuración de ración del evento.
        var rationConfig = await dbContext.RationConfigs!
            .FirstOrDefaultAsync(r => r.EventId == eventId, cancellationToken);

        if (rationConfig is null)
        {
            rationConfig = new RationConfig
            {
                Id = Guid.NewGuid(),
                OrganizationId = participant.OrganizationId,
                EventId = eventId,
                Name = "General",
                TotalAllowedPerParticipant = 1,
            };
            dbContext.RationConfigs!.Add(rationConfig);
        }

        // 4. Contar raciones ya consumidas por el participante en esta configuración.
        int consumed = await dbContext.CheckIns!
            .CountAsync(
                c => c.EventId == eventId
                  && c.ParticipantId == participantId
                  && c.RationConfigId == rationConfig.Id,
                cancellationToken);

        if (consumed >= rationConfig.TotalAllowedPerParticipant)
        {
            return new QrValidationResponse
            {
                Success = false,
                Message = $"Ración ya consumida (límite alcanzado: {rationConfig.TotalAllowedPerParticipant}).",
                ParticipantId = participantId,
                ParticipantName = participant.FullName,
                RationsConsumed = consumed,
            };
        }

        // 5. Registrar el check-in / consumo de ración.
        dbContext.CheckIns!.Add(new CheckIn
        {
            Id = Guid.NewGuid(),
            OrganizationId = participant.OrganizationId,
            EventId = eventId,
            ParticipantId = participantId,
            RationConfigId = rationConfig.Id,
            ScannedAt = DateTime.UtcNow,
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        return new QrValidationResponse
        {
            Success = true,
            Message = $"Ración validada para {participant.FullName}.",
            ParticipantId = participantId,
            ParticipantName = participant.FullName,
            RationsConsumed = consumed + 1,
        };
    }

    private static bool TryParsePayload(string payload, out Guid eventId, out Guid participantId)
    {
        eventId = Guid.Empty;
        participantId = Guid.Empty;

        if (string.IsNullOrWhiteSpace(payload))
            return false;

        var parts = payload.Split(':');
        return parts.Length == 2
            && Guid.TryParse(parts[0], out eventId)
            && Guid.TryParse(parts[1], out participantId);
    }

    private static QrValidationResponse Fail(string message) => new()
    {
        Success = false,
        Message = message,
    };
}
