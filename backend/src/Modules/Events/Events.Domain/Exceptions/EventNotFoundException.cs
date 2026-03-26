namespace Events.Domain.Exceptions;

public class EventNotFoundException(Guid eventId) : Exception($"Event with ID {eventId} was not found.");