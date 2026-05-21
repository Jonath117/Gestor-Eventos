namespace Registration.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

using Registration.Domain.Entities;

public class RegistrationDbContext(DbContextOptions<RegistrationDbContext> options) : DbContext(options)
{
    public DbSet<Order>? Orders { get; set; }
    public DbSet<Participant>? Participants { get; set; }

    public DbSet<Code>? Codes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("registration");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RegistrationDbContext).Assembly);
    }
}