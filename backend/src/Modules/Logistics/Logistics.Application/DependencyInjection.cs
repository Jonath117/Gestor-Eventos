using Logistics.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Logistics.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddLogisticsApplication(this IServiceCollection services)
    {
        services.AddScoped<QrGenerationService>();
        services.AddScoped<MailpitIntegrationService>();
        // IEventAccessService se registra en la capa de Infrastructure porque
        // su implementación depende del LogisticsDbContext.

        return services;
    }
}
