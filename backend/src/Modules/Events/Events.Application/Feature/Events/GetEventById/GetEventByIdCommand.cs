using MediatR;

namespace Events.Application.Feature.Events.GetEventById;

public record GetEventByIdQuery(Guid Id) : IRequest<GetEventByIdResponse>;

public record GetEventByIdResponse(Guid Id, string Name, DateTime Date);
