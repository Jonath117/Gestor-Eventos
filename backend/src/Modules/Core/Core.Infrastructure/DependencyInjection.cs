using Core.Application;
using Core.Application.Abstractions;
using Core.Application.Tenants;
using Core.Domain.Repositories;
using Core.Infrastructure.Persistence;
using Core.Infrastructure.Repositories;
using Core.Infrastructure.Tenants;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCoreApplication();

        string connectionString = configuration.GetConnectionString("NeonPostgres")
                                  ?? configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException("No se encontro la cadena de conexión 'NeonPostgres' o 'DefaultConnection'.");

        services.AddDbContext<CoreDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "core");
                })
                .UseSnakeCaseNamingConvention());

        services.AddScoped<ICoreDbContext>(sp => sp.GetRequiredService<CoreDbContext>());

        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITenantProvider, MockTenantProvider>();

        return services;
    }
}