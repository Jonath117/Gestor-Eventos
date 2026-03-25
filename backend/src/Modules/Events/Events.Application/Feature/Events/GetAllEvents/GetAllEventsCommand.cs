using MediatR;

namespace Events.Application.Feature.Events.GetAllEvents;

public record GetAllEventsQuery() : IRequest<IEnumerable<GetAllEventsResponse>>;

public record GetAllEventsResponse(Guid Id, string Name, DateTime Date);