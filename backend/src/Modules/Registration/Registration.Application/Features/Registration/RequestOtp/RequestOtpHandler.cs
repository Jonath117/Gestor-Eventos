using MediatR;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Registration.Application.Features.Registration.RequestOtp;

public class RequestOtpHandler(
    IMemoryCache cache,
    ILogger<RequestOtpHandler> logger) : IRequestHandler<RequestOtpCommand>
{
    public Task Handle(RequestOtpCommand request, CancellationToken cancellationToken)
    {
        // Generar un OTP de 6 dígitos aleatorio
        var otp = new Random().Next(100000, 999999).ToString();

        // Guardar en cache con expiración de 180 segundos (3 minutos)
        var cacheKey = $"OTP_{request.EventId}_{request.Email}";
        cache.Set(cacheKey, otp, TimeSpan.FromSeconds(180));

        // Simulación: Imprimir en consola para que el usuario pueda verlo
        logger.LogInformation("========================================");
        logger.LogInformation("SIMULACION OTP PARA REGISTRO");
        logger.LogInformation("Evento: {EventId}", request.EventId);
        logger.LogInformation("Email: {Email}", request.Email);
        logger.LogInformation("OTP GENERADO: {Otp}", otp);
        logger.LogInformation("Expira en: 180 segundos");
        logger.LogInformation("========================================");

        return Task.CompletedTask;
    }
}