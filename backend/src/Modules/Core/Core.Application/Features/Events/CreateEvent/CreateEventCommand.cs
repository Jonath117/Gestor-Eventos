using MediatR;

namespace Core.Application.Features.Events.CreateEvent;

public record CreateEventCommand(
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    int MaxCapacity,
    Guid OrganizationId) : IRequest<Guid>;