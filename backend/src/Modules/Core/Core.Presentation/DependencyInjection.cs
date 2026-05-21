using Microsoft.Extensions.DependencyInjection;

namespace Core.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddCorePresentation(this IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(DependencyInjection).Assembly);

        return services;
    }
}