using Microsoft.EntityFrameworkCore;

using Registration.Application.Interfaces;
using Registration.Domain.Entities;

namespace Registration.Infrastructure.Database;

public class RegistrationDbContext(DbContextOptions<RegistrationDbContext> options) : DbContext(options), IRegistrationDbContext
{
    public DbSet<Order>? Orders { get; set; }
    public DbSet<Participant>? Participants { get; set; }

    public DbSet<Code>? Codes { get; set; }
    public DbSet<OtpRequest>? OtpRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("registration");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RegistrationDbContext).Assembly);
    }
}