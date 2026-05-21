using Core.Domain.Entities;

namespace Core.Domain.Repositories;

public interface IEventRepository
{
    Task AddAsync(Event newEvent, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetAllByOrganizationAsync(Guid organizationId, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdAsync(Guid organizationId, Guid eventId, CancellationToken cancellationToken = default);

    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Event @event, CancellationToken cancellationToken = default);
}