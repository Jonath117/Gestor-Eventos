using Google.Api.Gax;
using Google.Cloud.PubSub.V1;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Registration.Application;
using Registration.Application.Interfaces;
using Registration.Infrastructure.Database;
using Registration.Infrastructure.Messaging;

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

        RegisterPubSub(services, configuration);

        return services;
    }

    private static void RegisterPubSub(IServiceCollection services, IConfiguration configuration)
    {
        var projectId = configuration["PubSub:ProjectId"];
        var topicId = configuration["PubSub:TopicId"];
        var emulatorHost = configuration["PubSub:EmulatorHost"];

        if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(topicId))
        {
            services.AddSingleton<IOtpMessagePublisher, NoOpOtpMessagePublisher>();
            return;
        }

        var topicName = TopicName.FromProjectTopic(projectId, topicId);

        if (!string.IsNullOrEmpty(emulatorHost))
        {
            Environment.SetEnvironmentVariable("PUBSUB_EMULATOR_HOST", emulatorHost);
        }

        services.AddSingleton(_ =>
            PublisherClient.CreateAsync(topicName).GetAwaiter().GetResult());

        services.AddSingleton<IOtpMessagePublisher, PubSubOtpMessagePublisher>();
    }
}