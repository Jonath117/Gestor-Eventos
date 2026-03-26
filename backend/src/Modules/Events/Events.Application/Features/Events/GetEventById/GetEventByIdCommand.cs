using MediatR;

namespace Events.Application.Features.Events.GetEventById;

public record GetEventByIdQuery(Guid Id) : IRequest<GetEventByIdResponse>;

public record GetEventByIdResponse(Guid Id, string Name, DateTime Date, int MaxCapacity, int CurrentParticipantsCount);
