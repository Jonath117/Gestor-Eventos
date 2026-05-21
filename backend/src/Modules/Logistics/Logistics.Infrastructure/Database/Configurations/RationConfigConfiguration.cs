namespace Logistics.Infrastructure.Database.Configurations;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RationConfigConfiguration : IEntityTypeConfiguration<RationConfig>
{
    public void Configure(EntityTypeBuilder<RationConfig> builder)
    {
        builder.ToTable("ration_configs");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name).IsRequired().HasMaxLength(255);
        builder.Property(r => r.TotalAllowedPerParticipant).HasDefaultValue(1);

        builder.HasData(new RationConfig
        {
            Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            OrganizationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            EventId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            Name = "Almuerzo Día 1",
            TotalAllowedPerParticipant = 1
        });
    }
}