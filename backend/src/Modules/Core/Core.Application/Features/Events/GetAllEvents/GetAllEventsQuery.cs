using MediatR;

namespace Core.Application.Features.Events.GetAllEvents;

public record GetAllEventsQuery() : IRequest<IEnumerable<GetAllEventsResponse>>;

public record GetAllEventsResponse(Guid Id, string Name, DateTime StartDate, DateTime EndDate);