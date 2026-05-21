using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Registration.Infrastructure.Database;

namespace Registration.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRegistrationInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("NeonPostgres") 
                                  ?? throw new InvalidOperationException("No se encontro la cadena de conexión 'NeonPostgres'.");
        
        services.AddDbContext<RegistrationDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions => 
                {
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "registration");
                })
                .UseSnakeCaseNamingConvention());
        return services;
    }
}