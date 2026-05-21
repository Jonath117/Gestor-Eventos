using Payment.Domain.Enums;

namespace Payment.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }

    public Guid OrderId { get; set; }

    public decimal Amount { get; set; }
    public TransactionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<ManualReceipt> ManualReceipts { get; set; } = [];
}