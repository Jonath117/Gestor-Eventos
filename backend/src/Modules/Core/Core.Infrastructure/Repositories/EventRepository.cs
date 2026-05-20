using Core.Domain.Entities;
using Core.Domain.Repositories;
using Core.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories;

public class EventRepository(CoreDbContext context) : IEventRepository
{
    public async Task AddAsync(Event newEvent, CancellationToken cancellationToken = default)
    {
        await context.Events.AddAsync(newEvent, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetAllByOrganizationAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        return await context.Events
            .Where(e => e.OrganizationId == organizationId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Event?> GetByIdAsync(Guid organizationId, Guid eventId, CancellationToken cancellationToken = default)
    {
        return await context.Events
            .FirstOrDefaultAsync(e => e.Id == eventId && e.OrganizationId == organizationId, cancellationToken);
    }

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Events
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Event @event, CancellationToken cancellationToken = default)
    {
        context.Events.Update(@event);
        await context.SaveChangesAsync(cancellationToken);
    }
}
