namespace Logistics.Application.Services;

public class MailpitIntegrationService
{
    public Task SendQrEmailAsync(Guid participantId, string qrPayload)
    {
        // Dummy implementation for MVP mailpit simulation
        Console.WriteLine($"[Mailpit Simulation] Sending QR to participant {participantId} with payload {qrPayload}");
        return Task.CompletedTask;
    }
}
