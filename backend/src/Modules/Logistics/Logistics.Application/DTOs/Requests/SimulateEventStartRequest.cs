namespace Logistics.Application.DTOs.Requests;

public class SimulateEventStartRequest
{
    public Guid EventId { get; set; }
    public List<Guid> ParticipantIds { get; set; } = new();
}
