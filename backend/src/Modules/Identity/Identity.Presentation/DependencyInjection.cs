using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

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
