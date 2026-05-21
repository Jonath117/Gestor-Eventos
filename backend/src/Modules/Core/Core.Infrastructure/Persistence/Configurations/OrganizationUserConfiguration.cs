namespace Core.Infrastructure.Persistence.Configurations;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrganizationUserConfiguration : IEntityTypeConfiguration<OrganizationUser>
{
    public void Configure(EntityTypeBuilder<OrganizationUser> builder)
    {
        builder.ToTable("organization_users");

        // Llave primaria compuesta
        builder.HasKey(ou => new { ou.OrganizationId, ou.UserId });

        builder.Property(ou => ou.Role)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ou => ou.JoinedAt)
            .HasDefaultValueSql("now()");

        // Relación con Organization
        builder.HasOne(ou => ou.Organization)
            .WithMany(o => o.OrganizationUsers)
            .HasForeignKey(ou => ou.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación con User
        builder.HasOne(ou => ou.User)
            .WithMany(u => u.OrganizationUsers)
            .HasForeignKey(ou => ou.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}