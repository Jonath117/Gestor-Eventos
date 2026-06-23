using Microsoft.Extensions.DependencyInjection;
using Logistics.Application;

namespace Logistics.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddLogisticsPresentation(this IServiceCollection services)
    {
        services.AddLogisticsApplication();
        return services;
    }
}
