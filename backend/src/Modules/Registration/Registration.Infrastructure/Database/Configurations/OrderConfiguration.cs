namespace Registration.Infrastructure.Database.Configurations;

using Domain.Entities;
using Domain.Enums;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Status)
            .HasConversion<string>();

        builder.Property(o => o.CreatedAt)
            .HasDefaultValueSql("now()");


        var sampleOrderId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        builder.HasData(new Order
        {
            Id = sampleOrderId,
            OrganizationId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            EventId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            ContactEmail = "usuarioprueba40@gmail.com",
            Status = OrderStatus.Confirmed,
            CreatedAt = new DateTime(2026, 5, 20, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}