namespace Logistics.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;

public class LogisticsDbContext(DbContextOptions<LogisticsDbContext> options) : DbContext(options)
{
    public DbSet<RationConfig> RationConfigs { get; set; }
    public DbSet<CheckIn> CheckIns { get; set; }
    public DbSet<OfflineSyncProjection> OfflineSyncProjections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("logistics"); 
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogisticsDbContext).Assembly);
    }
}