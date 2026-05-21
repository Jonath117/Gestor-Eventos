namespace Registration.Infrastructure.Database;

using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class RegistrationDbContextFactory : IDesignTimeDbContextFactory<RegistrationDbContext>
{
    public RegistrationDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", optional: false)
            .AddUserSecrets<RegistrationDbContextFactory>()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<RegistrationDbContext>();

        string connectionString = configuration.GetConnectionString("NeonPostgres")
                                  ?? throw new InvalidOperationException("No se encontró la cadena de conexión.");

        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "registration");
            })
            .UseSnakeCaseNamingConvention();

        return new RegistrationDbContext(optionsBuilder.Options);
    }
}