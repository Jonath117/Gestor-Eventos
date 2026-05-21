using Core.Application.Abstractions;
using Core.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence;

public class CoreDbContext(DbContextOptions<CoreDbContext> options) : DbContext(options), ICoreDbContext
{
    public DbSet<Organization> Organizations { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<OrganizationUser> OrganizationUsers { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("core");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreDbContext).Assembly);
    }
}