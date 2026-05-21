using Core.Application.Abstractions;
using Core.Application.Tenants;
using Core.Domain.Entities;
using Core.Domain.Repositories;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.Events.GetAllEvents;

public class GetAllEventsHandler(
    ICoreDbContext context, 
    ITenantProvider tenantProvider) : IRequestHandler<GetAllEventsQuery, IEnumerable<GetAllEventsResponse>>
{
    public async Task<IEnumerable<GetAllEventsResponse>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        Guid currentUserId = tenantProvider.GetCurrentUserId();

        // Buscamos todos los eventos de todas las organizaciones donde el usuario es miembro
        var events = await context.Events
            .AsNoTracking()
            .Where(e => context.OrganizationUsers
                .Any(ou => ou.OrganizationId == e.OrganizationId && ou.UserId == currentUserId))
            .OrderByDescending(e => e.StartDate)
            .ToListAsync(cancellationToken);

        return events.Select(evt => new GetAllEventsResponse(
            evt.Id, 
            evt.Name, 
            evt.StartDate, 
            evt.EndDate));
    }
}
