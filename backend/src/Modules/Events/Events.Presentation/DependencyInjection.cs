using Microsoft.Extensions.DependencyInjection;

namespace Events.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddEventsModule(this IServiceCollection services)
    {
        //services.AddEventsInfraestructure();
        
        
        return services;
    }
    
}