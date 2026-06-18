namespace Payment.Infrastructure.Database;

using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
{
    public PaymentDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddUserSecrets<PaymentDbContextFactory>()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();

        string connectionString = configuration.GetConnectionString("NeonPostgres")
                                  ?? configuration.GetConnectionString("DefaultConnection")
                                  ?? "Host=localhost;Database=dummy;Username=dummy;Password=dummy";

        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "payment");
            })
            .UseSnakeCaseNamingConvention();

        return new PaymentDbContext(optionsBuilder.Options);
    }
}