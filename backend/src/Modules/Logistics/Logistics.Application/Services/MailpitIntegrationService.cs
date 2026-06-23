namespace Logistics.Application.Services;

public class MailpitIntegrationService
{
    public Task SendQrEmailAsync(Guid participantId, string qrPayload, string? toEmail = null)
    {
        // Dummy implementation for MVP mailpit simulation
        var recipient = string.IsNullOrWhiteSpace(toEmail) ? participantId.ToString() : toEmail;
        Console.WriteLine(
            $"[Mailpit Simulation] Sending QR to {recipient} (participant {participantId}) with payload {qrPayload}");
        return Task.CompletedTask;
    }
}
