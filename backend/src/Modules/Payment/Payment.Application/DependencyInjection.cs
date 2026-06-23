using Microsoft.Extensions.DependencyInjection;
using Payment.Application.Services;

namespace Payment.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentApplication(this IServiceCollection services)
    {
        services.AddScoped<LocalAttachmentStorageService>();
        return services;
    }
}
