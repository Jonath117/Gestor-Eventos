namespace Logistics.Domain.Entities;

public class RationConfig
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid EventId { get; set; }

    public string Name { get; set; } = string.Empty;
    public int TotalAllowedPerParticipant { get; set; } = 1;

    public List<CheckIn> CheckIns { get; set; } = [];
}