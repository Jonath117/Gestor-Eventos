using System;

namespace Registration.Domain.Entities;

public class OtpRequest
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string Status { get; set; } = "pendiente"; // "pendiente", "procesado"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
}
