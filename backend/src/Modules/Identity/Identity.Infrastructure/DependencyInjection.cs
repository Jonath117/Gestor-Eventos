using System.Text;

using Identity.Application;
using Identity.Application.Interfaces;
using Identity.Domain.Repositories;
using Identity.Infrastructure.Authentication;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Repositories;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityApplication();
        services.AddIdentityInfrastructure(configuration);

        return services;
    }

    private static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        var connectionString = configuration.GetConnectionString("NeonPostgres")
                               ?? configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'NeonPostgres' o 'DefaultConnection'.");
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                })
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret))
            });

        return services;
    }
}