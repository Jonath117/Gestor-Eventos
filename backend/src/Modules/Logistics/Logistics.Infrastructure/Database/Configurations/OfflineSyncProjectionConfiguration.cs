namespace Logistics.Infrastructure.Database.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Logistics.Domain.Entities;

public class OfflineSyncProjectionConfiguration : IEntityTypeConfiguration<OfflineSyncProjection>
{
    public void Configure(EntityTypeBuilder<OfflineSyncProjection> builder)
    {
        builder.ToTable("offline_sync_projections");
        
        builder.HasKey(o => o.ParticipantId);

        builder.Property(o => o.QrIdentifier).IsRequired().HasMaxLength(255);
        builder.Property(o => o.FullName).IsRequired().HasMaxLength(255);

        builder.HasData(new OfflineSyncProjection
        {
            ParticipantId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            EventId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            QrIdentifier = "HASH_QR_SEGURO_001",
            FullName = "Jonathan Rocha",
            IsConfirmed = true,
            LastUpdatedAt = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}