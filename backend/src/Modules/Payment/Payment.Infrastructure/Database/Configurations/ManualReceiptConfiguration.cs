namespace Payment.Infrastructure.Database.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Payment.Domain.Entities;

public class ManualReceiptConfiguration : IEntityTypeConfiguration<ManualReceipt>
{
    public void Configure(EntityTypeBuilder<ManualReceipt> builder)
    {
        builder.ToTable("manual_receipts");
        builder.HasKey(m => m.Id);

        builder.HasIndex(m => m.FileHash).IsUnique();

        builder.Property(m => m.FileUrl).IsRequired().HasMaxLength(500);
        builder.Property(m => m.MimeType).IsRequired().HasMaxLength(50);
        builder.Property(m => m.UploadedAt).HasDefaultValueSql("now()");

        builder.HasOne(m => m.Transaction)
            .WithMany(t => t.ManualReceipts)
            .HasForeignKey(m => m.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(new ManualReceipt
        {
            Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            TransactionId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
            FileUrl = "https://s3.amazonaws.com/tu-bucket/comprobante_001.jpg",
            FileHash = "HASH_IMAGEN_SHA256_ABC123",
            MimeType = "image/jpeg",
            UploadedAt = new DateTime(2026, 5, 20, 1, 5, 0, DateTimeKind.Utc)
        });
    }
}