using System.Net.Mail;
using System.Net.Mime;

namespace Logistics.Application.Services;

public class MailpitIntegrationService
{
    public async Task SendQrEmailAsync(Guid participantId, byte[] qrImageBytes, string? toEmail = null)
    {
        var recipient = string.IsNullOrWhiteSpace(toEmail) ? $"{participantId}@example.com" : toEmail;
        
        using var client = new SmtpClient("localhost", 1025);
        
        using var message = new MailMessage();
        message.From = new MailAddress("noreply@gestoreventos.com", "Gestor Eventos");
        message.To.Add(new MailAddress(recipient));
        message.Subject = "Tu Código QR de Acceso al Evento";
        message.Body = "<p>¡Felicidades! Has sido aceptado. Adjunto encontrarás tu código QR de acceso.</p>";
        message.IsBodyHtml = true;

        using var stream = new MemoryStream(qrImageBytes);
        var attachment = new Attachment(stream, "qr-acceso.png", "image/png");
        message.Attachments.Add(attachment);

        Console.WriteLine($"[Mailpit Integration] Sending real email to {recipient} (participant {participantId}) with attached QR image.");
        await client.SendMailAsync(message);
    }
}
