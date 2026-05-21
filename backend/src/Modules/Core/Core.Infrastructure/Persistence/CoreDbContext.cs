namespace Core.Infrastructure.Persistence;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;

public class CoreDbContext(DbContextOptions<CoreDbContext> options) : DbContext(options)
{
    public DbSet<Organization>? Organizations { get; set; }
    public DbSet<User>? Users { get; set; }
    public DbSet<OrganizationUser>? OrganizationUsers { get; set; }
    public DbSet<Event>? Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("core");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreDbContext).Assembly);
    }
}