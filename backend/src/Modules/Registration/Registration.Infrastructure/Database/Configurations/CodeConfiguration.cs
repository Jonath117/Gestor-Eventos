namespace Registration.Infrastructure.Database.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Registration.Domain.Entities;

public class CodeConfiguration : IEntityTypeConfiguration<Code>
{
    public void Configure(EntityTypeBuilder<Code> builder)
    {
        builder.ToTable("codes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Token)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.Token)
            .IsUnique();

        builder.Property(c => c.IsUsed)
            .HasDefaultValue(false);

        builder.HasOne(c => c.UsedByParticipant)
            .WithMany()
            .HasForeignKey(c => c.UsedByParticipantId)
            .OnDelete(DeleteBehavior.SetNull);



        builder.HasData(new Code
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            OrganizationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            EventId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            Token = "BECA-UCB-100",
            IsUsed = false,
            UsedByParticipantId = null,
            UsedAt = null
        });

        builder.HasData(new Code
        {
            Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            OrganizationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            EventId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            Token = "DESC-50-VIP",
            IsUsed = true,
            UsedByParticipantId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            UsedAt = new DateTime(2026, 5, 20, 10, 30, 0, DateTimeKind.Utc)
        });
    }
}