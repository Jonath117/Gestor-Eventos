using Events.Application.Tenants;
using Events.Domain.Entities;
using Events.Domain.Repositories;
using MediatR;

namespace Events.Application.Features.Events.GetEventById;

public class GetEventByIdHandler(
    IEventRepository repository,
    ITenantProvider tenantProvider
    ) : IRequestHandler<GetEventByIdQuery, GetEventByIdResponse>
{
    public async Task<GetEventByIdResponse> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        Guid tenantId = tenantProvider.GetTenantId();

        Event? eventEntity = await repository.GetByIdAsync(tenantId, request.Id, cancellationToken);

        if (eventEntity is null)
        {
            throw new Exception($"No se encontro el evento {request.Id}");
        }
        
        return new GetEventByIdResponse(eventEntity.Id, eventEntity.Name, eventEntity.Date);
    }
}
