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
            .Include(e => e.Participants) // Cargar los participantes para poder calcular CurrentParticipantsCount
            .FirstOrDefaultAsync(e => e.Id == eventId && e.TenantId == tenantId, cancellationToken);
    }

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Events
            .Include(e => e.Participants) // Cargar los participantes para validar la capacidad en el dominio
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Event @event, CancellationToken cancellationToken = default)
    {
        foreach (Participant participant in @event.Participants)
        {
            var entry = context.Entry(participant);
            
            if (entry.State == EntityState.Detached || entry.State == EntityState.Modified)
            {
                bool exists = await context.Participants
                    .AsNoTracking()
                    .AnyAsync(p => p.Id == participant.Id, cancellationToken);

                if (!exists)
                {
                    entry.State = EntityState.Added;
                }
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
