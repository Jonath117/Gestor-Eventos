using MediatR;
using Microsoft.EntityFrameworkCore;
using Registration.Application.Interfaces;
using Registration.Domain.Enums;

namespace Registration.Application.Features.Registration.GetEventSummary;

public class GetEventSummaryHandler(IRegistrationDbContext dbContext)
    : IRequestHandler<GetEventSummaryQuery, EventSummaryDto>
{
    public async Task<EventSummaryDto> Handle(GetEventSummaryQuery request, CancellationToken cancellationToken)
    {
        var confirmedParticipants = await dbContext.Orders!
            .Where(o => o.EventId == request.EventId && o.Status == OrderStatus.Confirmed)
            .SelectMany(o => o.Participants)
            .CountAsync(cancellationToken);

        return new EventSummaryDto(confirmedParticipants);
    }
}
