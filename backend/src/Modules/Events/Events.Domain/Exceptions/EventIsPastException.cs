namespace Events.Domain.Exceptions;

public class EventIsPastException() : Exception("Cannot register for a past event.");