namespace Registration.Infrastructure.Database.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("participants");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.FullName).IsRequired().HasMaxLength(255);
        
        builder.HasIndex(p => p.QrIdentifier).IsUnique();

        builder.HasOne(p => p.Order)
            .WithMany(o => o.Participants)
            .HasForeignKey(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(new Participant
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            OrderId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            FullName = "Jonathan Rocha",
            Phone = "77712345",
            QrIdentifier = "HASH_QR_SEGURO_001"
        });
    }
}