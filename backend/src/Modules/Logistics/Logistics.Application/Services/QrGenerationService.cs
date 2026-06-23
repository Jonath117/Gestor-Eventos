namespace Logistics.Application.Services;

public class QrGenerationService
{
    public string GenerateParticipantQrPayload(Guid participantId, Guid eventId)
    {
        // Dummy implementation for MVP. In reality, this would be a signed JWT.
        return $"{eventId}:{participantId}";
    }
}
