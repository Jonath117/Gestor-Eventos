namespace Events.Domain.Exceptions;

public class EventNotFoundException : DomainException
{
    public EventNotFoundException(Guid eventId)
        : base($"No se encontro ningun evento con el id {eventId}") { }
}