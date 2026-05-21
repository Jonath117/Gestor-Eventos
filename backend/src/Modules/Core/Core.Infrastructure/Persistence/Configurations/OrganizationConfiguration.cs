namespace Core.Infrastructure.Persistence.Configurations;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("organizations");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(o => o.QrPaymentImageUrl)
            .HasMaxLength(1000);

        builder.Property(o => o.CreatedAt)
            .HasDefaultValueSql("now()");
    }
}