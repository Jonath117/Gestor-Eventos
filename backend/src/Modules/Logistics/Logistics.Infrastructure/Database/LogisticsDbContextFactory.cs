namespace Logistics.Infrastructure.Database;

using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class LogisticsDbContextFactory : IDesignTimeDbContextFactory<LogisticsDbContext>
{
    public LogisticsDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", optional: false)
            .AddUserSecrets<LogisticsDbContextFactory>()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<LogisticsDbContext>();

        string connectionString = configuration.GetConnectionString("NeonPostgres")
                                  ?? throw new InvalidOperationException("No se encontró la cadena de conexión.");

        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "logistics");
            })
            .UseSnakeCaseNamingConvention();

        return new LogisticsDbContext(optionsBuilder.Options);
    }
}