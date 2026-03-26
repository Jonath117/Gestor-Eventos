namespace Events.Domain.Exceptions;

public class EventIsFullException() : Exception("The event has reached its maximum capacity.");