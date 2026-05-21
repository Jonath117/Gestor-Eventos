using Registration.Domain.Enums;

namespace Registration.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; } 
    public Guid EventId { get; set; }        
    
    public string ContactEmail { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<Participant> Participants { get; set; } = [];
}