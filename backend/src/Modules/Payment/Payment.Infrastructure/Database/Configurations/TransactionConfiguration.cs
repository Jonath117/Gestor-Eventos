namespace Payment.Infrastructure.Database.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Payment.Domain.Entities;
using Payment.Domain.Enums;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.OrderId).IsUnique();

        builder.Property(t => t.Amount).HasColumnType("decimal(10,2)");

        builder.Property(t => t.Status).HasConversion<string>();

        builder.Property(t => t.CreatedAt).HasDefaultValueSql("now()");

        builder.HasData(new Transaction
        {
            Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
            OrganizationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            OrderId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Amount = 150.00m,
            Status = TransactionStatus.Verified,
            CreatedAt = new DateTime(2026, 5, 20, 1, 0, 0, DateTimeKind.Utc)
        });
    }
}