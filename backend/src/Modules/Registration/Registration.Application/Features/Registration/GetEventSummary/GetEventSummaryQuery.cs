using MediatR;

namespace Registration.Application.Features.Registration.GetEventSummary;

public record EventSummaryDto(int ConfirmedParticipants);

public record GetEventSummaryQuery(Guid EventId) : IRequest<EventSummaryDto>;