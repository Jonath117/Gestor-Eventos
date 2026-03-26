using Events.Application.Tenants;
using Events.Domain.Entities;
using Events.Domain.Repositories;

using MediatR;

namespace Events.Application.Feature.Events.GetAllEvents;

public class GetAllEventsHandler (
    IEventRepository repository, ITenantProvider tenantProvider) : IRequestHandler<GetAllEventsQuery, IEnumerable<GetAllEventsResponse>> 
{
    public async Task<IEnumerable<GetAllEventsResponse>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        Guid tenantId = tenantProvider.GetTenantId();
        IEnumerable<Event> events = await repository.GetAllByTenantAsync(tenantId, cancellationToken);

        List<GetAllEventsResponse> response = [];
        
        response.AddRange(events.Select(evt => new GetAllEventsResponse(evt.Id, evt.Name, evt.Date)));

        return response;
    }
}