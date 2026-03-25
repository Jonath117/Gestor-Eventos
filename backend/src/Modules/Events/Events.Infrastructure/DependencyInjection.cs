using Events.Application.Tenant;
using Events.Infrastructure.Database;
using Events.Infrastructure.Tenant;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddEventsInfrastructure(this ServiceCollection services)
    {
        //registr bdd In-Memory
        services.AddDbContext<EventsDbContext>(options =>
            options.UseInMemoryDatabase("EventosSaas_MockDb"));
        
        //registar Mock tenant provider
        services.AddScoped<ITenantProvider, MockTenantProvider>();

        return services;
    }
}