using Events.Domain.Entities;

namespace Events.Domain.Repositories;

public interface IEventRepository
{
    Task AddAsync(Event newEvent, CancellationToken cancellationToken = default);
    Task<IEnumerable<Event>> GetAllByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdAsync(Guid tenantId, Guid eventId, CancellationToken cancellationToken = default);

    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Event @event, CancellationToken cancellationToken = default);
}