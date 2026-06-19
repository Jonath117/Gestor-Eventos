using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Registration.Application.Interfaces;
using Registration.Domain.Entities;

namespace Registration.Infrastructure.Messaging;

public class NoOpOtpMessagePublisher(
    IServiceScopeFactory scopeFactory,
    ILogger<NoOpOtpMessagePublisher> logger) : IOtpMessagePublisher
{
    public async Task PublishOtpRequestAsync(OtpRequest otpRequest, CancellationToken cancellationToken = default)
    {
        logger.LogWarning(
            "Pub/Sub no configurado. Iniciando simulación local de OTP para la solicitud {RequestId} de {Email}.",
            otpRequest.Id,
            otpRequest.UserId);

        // 1. Generar código OTP de 6 dígitos
        var otpCode = new Random().Next(100000, 999999).ToString();

        try
        {
            // 2. Persistir el código generado en la base de datos y cambiar estado a 'procesado'
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IRegistrationDbContext>();

                var record = await dbContext.OtpRequests!
                    .FirstOrDefaultAsync(o => o.Id == otpRequest.Id, cancellationToken);

                if (record != null)
                {
                    record.Code = otpCode;
                    record.Status = "procesado";
                    record.ProcessedAt = DateTime.UtcNow;

                    await dbContext.SaveChangesAsync(cancellationToken);

                    logger.LogInformation(
                        "Simulación local: Código OTP ({Code}) guardado en la base de datos con estado PROCESADO para la solicitud {RequestId}.",
                        otpCode,
                        otpRequest.Id);
                }
                else
                {
                    logger.LogError(
                        "Simulación local errónea: No se encontró la solicitud de OTP con ID {RequestId} en la base de datos.",
                        otpRequest.Id);
                    return;
                }
            }

            // 3. Enviar correo SMTP local a Mailpit (puerto 1025)
            using (var smtpClient = new SmtpClient("127.0.0.1", 1025))
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("noreply@campeando.com", "Campeando Eventos"),
                    Subject = "Tu código de verificación OTP (Entorno Local)",
                    Body = $"<html><body><h2>Código de Verificación</h2><p>Tu código OTP local es: <strong>{otpCode}</strong></p></body></html>",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(otpRequest.UserId);

                await smtpClient.SendMailAsync(mailMessage, cancellationToken);

                logger.LogInformation(
                    "Simulación local: Correo de verificación enviado con éxito a Mailpit para {Email}.",
                    otpRequest.UserId);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error durante la simulación local de OTP para {Email}: {Message}",
                otpRequest.UserId,
                ex.Message);
        }
    }
}