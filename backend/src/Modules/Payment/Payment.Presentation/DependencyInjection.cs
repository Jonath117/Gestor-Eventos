using Microsoft.Extensions.DependencyInjection;

namespace Payment.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentPresentation(this IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(DependencyInjection).Assembly);

        return services;
    }
}
