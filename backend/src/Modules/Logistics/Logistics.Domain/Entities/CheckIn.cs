namespace Logistics.Domain.Entities;

public class CheckIn
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid EventId { get; set; }
    public Guid ParticipantId { get; set; }
    
    public Guid? RationConfigId { get; set; }
    public RationConfig? RationConfig { get; set; } 
    
    public DateTime ScannedAt { get; set; }
    public string? OfflineSyncId { get; set; }
}