using Core.Application.Abstractions;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.Events.GetAllEvents;

public class GetAllEventsHandler(ICoreDbContext context) : IRequestHandler<GetAllEventsQuery, IEnumerable<GetAllEventsResponse>>
{
    public async Task<IEnumerable<GetAllEventsResponse>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await context.Events
            .AsNoTracking()
            .Where(e => e.OrganizationId == request.OrganizationId)
            .OrderByDescending(e => e.StartDate)
            .ToListAsync(cancellationToken);

        return events.Select(evt => new GetAllEventsResponse(
            evt.Id,
            evt.Name,
            evt.StartDate,
            evt.EndDate));
    }
}