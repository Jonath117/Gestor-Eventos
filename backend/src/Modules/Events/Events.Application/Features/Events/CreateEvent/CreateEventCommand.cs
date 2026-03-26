using MediatR;

namespace Events.Application.Features.Events.CreateEvent;

public record CreateEventCommand(string Name, DateTime Date, int MaxCapacity) : IRequest<Guid>;