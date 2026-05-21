namespace Payment.Domain.Entities;

public class ManualReceipt
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; } = null!;
    
    public string FileUrl { get; set; } = string.Empty;
    public string FileHash { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}