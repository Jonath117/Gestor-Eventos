using MediatR;

using Events.Application.Tenants;
using Events.Domain.Entities;
using Events.Domain.Repositories;

namespace Events.Application.Features.Events.CreateEvent;

public class CreateEventHandler(
        IEventRepository repository,
        ITenantProvider tenantProvider) : IRequestHandler <CreateEventCommand, Guid> 
{
    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        Guid tenantId = tenantProvider.GetTenantId();

        Event newEvent = new()
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = request.Name,
            Date = request.Date,
            MaxCapacity = request.MaxCapacity
        };
        
        await repository.AddAsync(newEvent, cancellationToken);
        
        return newEvent.Id;
    }
    
}