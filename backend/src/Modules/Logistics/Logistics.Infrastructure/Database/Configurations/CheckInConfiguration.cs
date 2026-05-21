namespace Logistics.Infrastructure.Database.Configurations;

using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CheckInConfiguration : IEntityTypeConfiguration<CheckIn>
{
    public void Configure(EntityTypeBuilder<CheckIn> builder)
    {
        builder.ToTable("check_ins");
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.OfflineSyncId).IsUnique();

        builder.HasOne(c => c.RationConfig)
            .WithMany(r => r.CheckIns)
            .HasForeignKey(c => c.RationConfigId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(new CheckIn
        {
            Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
            OrganizationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            EventId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            ParticipantId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            RationConfigId = null,
            ScannedAt = new DateTime(2026, 5, 20, 8, 0, 0, DateTimeKind.Utc),
            OfflineSyncId = "sync_001_mobile"
        });
    }
}