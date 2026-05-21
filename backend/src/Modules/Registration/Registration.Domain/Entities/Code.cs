namespace Registration.Domain.Entities;

public class Code
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid EventId { get; set; }
    
    public string Token { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
    
    public Guid? UsedByParticipantId { get; set; }
    public Participant? UsedByParticipant { get; set; }
    
    public DateTime? UsedAt { get; set; }
}