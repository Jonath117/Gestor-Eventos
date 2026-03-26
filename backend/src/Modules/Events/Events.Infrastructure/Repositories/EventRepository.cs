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
        // Detectar participantes nuevos que aún no están siendo rastreados por el DbContext.
        // Esto es necesario porque el proveedor InMemory no detecta automáticamente
        // entidades nuevas añadidas a una colección de navegación cuando ya tienen un Id asignado.
        foreach (Participant participant in @event.Participants)
        {
            EntityState state = context.Entry(participant).State;
            if (state == EntityState.Detached)
            {
                await context.Participants.AddAsync(participant, cancellationToken);
            }
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
