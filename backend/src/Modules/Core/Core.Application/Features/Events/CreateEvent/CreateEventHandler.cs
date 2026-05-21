using Core.Application.Tenants;
using Core.Domain.Entities;
using Core.Domain.Repositories;

using MediatR;

namespace Core.Application.Features.Events.CreateEvent;

public class CreateEventHandler(
        IEventRepository repository,
        IOrganizationProvider tenantProvider) : IRequestHandler<CreateEventCommand, Guid>
{
    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        Guid organizationId = tenantProvider.GetTenantId();

        Event newEvent = new()
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            Name = request.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            MaxCapacity = request.MaxCapacity,
            CreatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(newEvent, cancellationToken);

        return newEvent.Id;
    }

}