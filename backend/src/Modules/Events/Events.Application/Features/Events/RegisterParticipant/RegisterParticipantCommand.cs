using MediatR;

namespace Events.Application.Features.Events.RegisterParticipant;

public record RegisterParticipantCommand(Guid EventId, string FullName, string Email) : IRequest<RegisterParticipantResponse>;
