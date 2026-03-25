using Microsoft.Extensions.DependencyInjection;
using Identity.Application.Features.Users.Login;
using Microsoft.Extensions.Configuration;

namespace Identity.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
                .AddApplicationPart(typeof(DependencyInjection).Assembly);

        services.AddMediatR(config => {
            config.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly);
        });
        
        return services;
    }
}
