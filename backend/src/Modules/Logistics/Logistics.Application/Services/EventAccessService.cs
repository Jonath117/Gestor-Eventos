using Logistics.Application.DTOs.Requests;
using Logistics.Application.DTOs.Responses;

namespace Logistics.Application.Services;

public class EventAccessService
{
    private readonly QrGenerationService _qrGenerationService;
    private readonly MailpitIntegrationService _mailService;

    public EventAccessService(QrGenerationService qrGenerationService, MailpitIntegrationService mailService)
    {
        _qrGenerationService = qrGenerationService;
        _mailService = mailService;
    }

    public async Task SimulateEventStartAsync(SimulateEventStartRequest request)
    {
        foreach (var participantId in request.ParticipantIds)
        {
            var payload = _qrGenerationService.GenerateParticipantQrPayload(participantId, request.EventId);
            await _mailService.SendQrEmailAsync(participantId, payload);
        }
    }

    public Task<QrValidationResponse> ValidateQrAsync(ValidateQrRequest request)
    {
        // Dummy implementation for MVP
        var success = !string.IsNullOrWhiteSpace(request.Payload);
        return Task.FromResult(new QrValidationResponse
        {
            Success = success,
            Message = success ? "Ración validada correctamente" : "QR Inválido o vacío"
        });
    }
}
