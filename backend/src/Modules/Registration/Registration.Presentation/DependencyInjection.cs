using Microsoft.Extensions.DependencyInjection;

namespace Registration.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddRegistrationPresentation(this IServiceCollection services)
    {
        services.AddControllers()
                .AddApplicationPart(typeof(DependencyInjection).Assembly);

        return services;
    }
}