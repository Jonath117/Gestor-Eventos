using Core.Application.Tenants;
using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Domain.Repositories;
using MediatR;

namespace Core.Application.Features.Events.GetEventById;

public class GetEventByIdHandler(
    IEventRepository repository,
    IOrganizationProvider tenantProvider
    ) : IRequestHandler<GetEventByIdQuery, GetEventByIdResponse>
{
    public async Task<GetEventByIdResponse> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        Guid organizationId = tenantProvider.GetTenantId();

        Event? eventEntity = await repository.GetByIdAsync(organizationId, request.Id, cancellationToken);

        if (eventEntity is null)
        {
            throw new EventNotFoundException(request.Id);
        }
        
        return new GetEventByIdResponse(
            eventEntity.Id, 
            eventEntity.Name, 
            eventEntity.StartDate,
            eventEntity.EndDate,
            eventEntity.MaxCapacity
        );
    }
}
