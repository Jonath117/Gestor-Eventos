using Microsoft.Extensions.DependencyInjection;

namespace Registration.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddRegistrationApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services.AddMemoryCache();
        services.AddScoped<Services.ApplicationQueryService>();

        return services;
    }
}