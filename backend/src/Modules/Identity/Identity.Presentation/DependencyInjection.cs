using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityPresentation(this IServiceCollection services)
    {
        services.AddControllers()
                .AddApplicationPart(typeof(DependencyInjection).Assembly);

        return services;
    }
}