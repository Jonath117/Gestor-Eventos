namespace Core.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

public class CoreDbContextFactory : IDesignTimeDbContextFactory<CoreDbContext>
{
    public CoreDbContext CreateDbContext(string[] args)
    {

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
        
        string connectionString = configuration.GetConnectionString("NeonPostgres")
                                  ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'NeonPostgres'.");
        
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "core");
            })
            .UseSnakeCaseNamingConvention();

        return new CoreDbContext(optionsBuilder.Options);
    }
}