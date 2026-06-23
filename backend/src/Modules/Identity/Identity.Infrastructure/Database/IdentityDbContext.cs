using Identity.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Database;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<OrganizationUser> OrganizationUsers => Set<OrganizationUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("core");

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("now()");

            entity.HasMany(u => u.RefreshTokens)
                  .WithOne()
                  .HasForeignKey(rt => rt.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(new
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Email = "admin@campeando.com",
                PasswordHash = "$2a$11$MGpEBiAytwzYQQZ/23CWRueAnvvyrFHumKraeMObdiqdEDOiP8FlG",
                CreatedAt = new DateTime(2026, 6, 23, 5, 49, 39, 955, DateTimeKind.Utc).AddTicks(7842)
            });
        });

        modelBuilder.Entity<OrganizationUser>(entity =>
        {
            entity.ToTable("organization_users");
            entity.HasKey(ou => new { ou.OrganizationId, ou.UserId });
            entity.Property(ou => ou.Role).IsRequired();
            entity.Property(ou => ou.JoinedAt).HasDefaultValueSql("now()");

            entity.HasOne<User>()
                  .WithMany(u => u.OrganizationUsers)
                  .HasForeignKey(ou => ou.UserId);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("user_refresh_tokens"); // Extensión del esquema core
            entity.HasKey(rt => rt.Token);
            entity.Property(rt => rt.Token).HasMaxLength(200);
            entity.Property(rt => rt.ReplacedByToken).HasMaxLength(200);
        });

        base.OnModelCreating(modelBuilder);
    }
}