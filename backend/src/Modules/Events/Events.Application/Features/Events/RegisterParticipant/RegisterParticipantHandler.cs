using Events.Domain.Entities;
using Events.Domain.Exceptions;
using Events.Domain.Repositories;
using MediatR;

namespace Events.Application.Features.Events.RegisterParticipant;

public class RegisterParticipantHandler(IEventRepository eventRepository) : IRequestHandler<RegisterParticipantCommand, RegisterParticipantResponse>
{
    public async Task<RegisterParticipantResponse> Handle(RegisterParticipantCommand request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(request.EventId, cancellationToken)
            ?? throw new EventNotFoundException(request.EventId);

        var participant = new Participant
        {
            EventId = request.EventId,
            FullName = request.FullName,
            Email = request.Email
        };

        @event.AddParticipant(participant);

        await eventRepository.UpdateAsync(@event, cancellationToken);

        return new RegisterParticipantResponse(participant.Id);
    }
}
