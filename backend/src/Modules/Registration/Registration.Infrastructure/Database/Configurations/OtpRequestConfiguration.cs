namespace Registration.Infrastructure.Database.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Registration.Domain.Entities;

public class OtpRequestConfiguration : IEntityTypeConfiguration<OtpRequest>
{
    public void Configure(EntityTypeBuilder<OtpRequest> builder)
    {
        builder.ToTable("otp_requests");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.UserId)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(o => o.TenantId)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(o => o.Code)
            .HasMaxLength(6);

        builder.Property(o => o.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("pendiente");

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.ProcessedAt);
    }
}
