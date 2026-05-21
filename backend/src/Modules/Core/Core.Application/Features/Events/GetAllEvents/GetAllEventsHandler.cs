using Core.Application.Tenants;
using Core.Domain.Entities;
using Core.Domain.Repositories;

using MediatR;

namespace Core.Application.Features.Events.GetAllEvents;

public class GetAllEventsHandler(
    IEventRepository repository, ITenantProvider tenantProvider) : IRequestHandler<GetAllEventsQuery, IEnumerable<GetAllEventsResponse>>
{
    public async Task<IEnumerable<GetAllEventsResponse>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        Guid organizationId = tenantProvider.GetTenantId();
        IEnumerable<Event> events = await repository.GetAllByOrganizationAsync(organizationId, cancellationToken);

        List<GetAllEventsResponse> response = [];

        response.AddRange(events.Select(evt => new GetAllEventsResponse(evt.Id, evt.Name, evt.StartDate, evt.EndDate)));

        return response;
    }
}