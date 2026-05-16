using Core.Infrastructure.Persistence;

namespace Core.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddCoreApplication();
        
        string connectionString = configuration.GetConnectionString("NeonPostgres") 
                                  ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'NeonPostgres'.");
        
        services.AddDbContext<CoreDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions => 
                {
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "core");
                })
                .UseSnakeCaseNamingConvention());
        
        
        return services;
    }
}