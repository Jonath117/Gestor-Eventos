using Events.Domain.Entities;
using Events.Domain.Repositories;
using Events.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;


namespace Events.Infrastructure.Repositories;

public class EventRepository(EventsDbContext context) : IEventRepository
{
    public async Task AddAsync(Event newEvent, CancellationToken cancellationToken = default)
    {
        await context.Events.AddAsync(newEvent, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Event>> GetAllByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await context.Events
            .Where(e => e.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }
    

    public async Task<Event?> GetByIdAsync(Guid tenantId, Guid eventId, CancellationToken cancellationToken = default)
    {
        return await context.Events
            .FirstOrDefaultAsync(e => e.Id == eventId && e.TenantId == tenantId, cancellationToken);
    }
}