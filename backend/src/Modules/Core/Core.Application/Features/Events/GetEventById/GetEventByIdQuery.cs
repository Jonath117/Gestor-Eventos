using MediatR;

namespace Core.Application.Features.Events.GetEventById;

public record GetEventByIdQuery(Guid Id) : IRequest<GetEventByIdResponse>;

public record GetEventByIdResponse(Guid Id, string Name, DateTime StartDate, DateTime EndDate, int MaxCapacity);