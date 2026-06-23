namespace Logistics.Infrastructure;

using Database;

using Logistics.Application.Services;
using Logistics.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddLogisticsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("NeonPostgres")
                                  ?? configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException("No se encontro la cadena de conexión 'NeonPostgres' o 'DefaultConnection'.");

        services.AddDbContext<LogisticsDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "logistics");
                })
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IEventMetricsService, EventMetricsService>();

        return services;
    }
}