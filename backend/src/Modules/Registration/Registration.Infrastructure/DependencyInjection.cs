using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Registration.Application;
using Registration.Application.Interfaces;
using Registration.Infrastructure.Database;

namespace Registration.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRegistrationModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRegistrationApplication();
        services.AddRegistrationInfrastructure(configuration);
        return services;
    }

    public static IServiceCollection AddRegistrationInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("NeonPostgres")
                                  ?? configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException("No se encontro la cadena de conexión 'NeonPostgres' o 'DefaultConnection'.");

        services.AddDbContext<RegistrationDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "registration");
                })
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IRegistrationDbContext>(provider =>
            provider.GetRequiredService<RegistrationDbContext>());

        return services;
    }
}