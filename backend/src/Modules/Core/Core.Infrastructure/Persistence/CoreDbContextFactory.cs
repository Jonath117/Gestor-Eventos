namespace Core.Infrastructure.Persistence;

using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class CoreDbContextFactory : IDesignTimeDbContextFactory<CoreDbContext>
{
    public CoreDbContext CreateDbContext(string[] args)
    {

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddUserSecrets<CoreDbContextFactory>()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();

        string connectionString = configuration.GetConnectionString("NeonPostgres")
                                  ?? configuration.GetConnectionString("DefaultConnection")
                                  ?? "Host=localhost;Database=dummy;Username=dummy;Password=dummy";

        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "core");
            })
            .UseSnakeCaseNamingConvention();

        return new CoreDbContext(optionsBuilder.Options);
    }
}