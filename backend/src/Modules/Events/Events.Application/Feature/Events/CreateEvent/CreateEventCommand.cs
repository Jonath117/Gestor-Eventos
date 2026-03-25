namespace Events.Application.Feature.Events.CreateEvent;

public record CreateEventCommand(string Name, DateTime Date, int MaxCapacity);