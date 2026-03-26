using Events.Domain.Exceptions;

namespace Events.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int MaxCapacity { get; set; }

    public List<Participant> Participants { get; set; } = [];
    
    public void AddParticipant(Participant participant)
    {
        if (Date < DateTime.UtcNow)
        {
            throw new EventIsPastException();
        }

        if (Participants.Count >= MaxCapacity)
        {
            throw new EventIsFullException();
        }

        Participants.Add(participant);
    }
}
