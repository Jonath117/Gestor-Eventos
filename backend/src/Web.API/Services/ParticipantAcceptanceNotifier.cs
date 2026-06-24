using Logistics.Application.Services;

using Registration.Application.Interfaces;

namespace Web.API.Services;

/// <summary>
/// Implementación de <see cref="IAcceptanceNotifier"/> en la capa de composición.
/// Genera el payload del QR (Logistics) y lo envía por correo al participante
/// aceptado, sin acoplar el módulo Registration con Logistics.
/// </summary>
public class ParticipantAcceptanceNotifier(
    QrGenerationService qrGenerationService,
    MailpitIntegrationService mailService) : IAcceptanceNotifier
{
    public async Task NotifyAcceptedAsync(
        Guid eventId,
        Guid participantId,
        string contactEmail,
        CancellationToken cancellationToken = default)
    {
        var payload = qrGenerationService.GenerateParticipantQrPayload(participantId, eventId);
        var qrImageBytes = qrGenerationService.GenerateQrImage(payload);
        await mailService.SendQrEmailAsync(participantId, qrImageBytes, contactEmail);
    }
}
