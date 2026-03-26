using Events.Application;
using Events.Application.Tenants;
using Events.Domain.Repositories;
using Events.Infrastructure.Database;
using Events.Infrastructure.Repositories;
using Events.Infrastructure.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddEventsModule(this IServiceCollection services)
    {
        services.AddEventsApplication();
        services.AddEventsInfrastructure();
        
        return services;
    }

    private static IServiceCollection AddEventsInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<EventsDbContext>(options =>
            options.UseInMemoryDatabase("EventosSaas_MockDb"));
        
        services.AddScoped<ITenantProvider, MockTenantProvider>();
        services.AddScoped<IEventRepository, EventRepository>();

        return services;
    }
}