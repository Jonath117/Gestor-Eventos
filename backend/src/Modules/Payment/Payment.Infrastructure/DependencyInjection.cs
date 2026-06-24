namespace Payment.Infrastructure;

using Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Payment.Application;
using Payment.Application.Abstractions;
using Payment.Infrastructure.Storage;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("NeonPostgres")
                                  ?? configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException("No se encontro la cadena de conexión 'NeonPostgres' o 'DefaultConnection'.");

        services.AddDbContext<PaymentDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "payment");
                })
                .UseSnakeCaseNamingConvention());
                
        services.AddPaymentApplication();

        services.AddSingleton(new StorageOptions
        {
            ServiceUrl = configuration[$"{StorageOptions.SectionName}:ServiceUrl"] ?? "http://localhost:9000",
            PublicBaseUrl = configuration[$"{StorageOptions.SectionName}:PublicBaseUrl"] ?? "http://localhost:9000",
            BucketName = configuration[$"{StorageOptions.SectionName}:BucketName"] ?? "gestor-eventos",
        });
        services.AddScoped<IAttachmentStorageService, MinioAttachmentStorageService>();
        services.AddScoped<IReceiptService, ReceiptService>();
        services.AddScoped<IReceiptReader, ReceiptReader>();
        return services;
    }
}