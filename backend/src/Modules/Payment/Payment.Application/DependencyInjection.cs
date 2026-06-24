using Microsoft.Extensions.DependencyInjection;
using Payment.Application.Services;

namespace Payment.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentApplication(this IServiceCollection services)
    {
        // Almacenamiento S3 se registrará en Infrastructure o API
        services.AddScoped<PaymentLinkGeneratorService>();
        return services;
    }
}
