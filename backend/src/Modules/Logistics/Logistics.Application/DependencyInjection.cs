using Logistics.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Logistics.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddLogisticsApplication(this IServiceCollection services)
    {
        services.AddScoped<QrGenerationService>();
        services.AddScoped<MailpitIntegrationService>();
        services.AddScoped<EventAccessService>();

        return services;
    }
}
