namespace Logistics.Domain.Entities;

public class OfflineSyncProjection
{
    public Guid ParticipantId { get; set; } 
    public Guid EventId { get; set; }
    
    public string QrIdentifier { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsConfirmed { get; set; }
    
    public DateTime? LastUpdatedAt { get; set; }
}