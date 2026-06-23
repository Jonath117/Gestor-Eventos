using Core.Application.Abstractions;

using Logistics.Application.DTOs.Responses;
using Logistics.Application.Services;
using Logistics.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Services;

/// <summary>
/// Calcula las métricas operativas de un evento cruzando los check-ins y
/// raciones registrados en Logistics con la capacidad máxima definida en Core.
/// </summary>
public class EventMetricsService(
    LogisticsDbContext logisticsDbContext,
    ICoreDbContext coreDbContext) : IEventMetricsService
{
    public async Task<EventMetricsResponse> GetEventMetricsAsync(
        Guid eventId,
        CancellationToken cancellationToken = default)
    {
        int totalCapacity = await coreDbContext.Events
            .AsNoTracking()
            .Where(e => e.Id == eventId)
            .Select(e => e.MaxCapacity)
            .FirstOrDefaultAsync(cancellationToken);

        IQueryable<Domain.Entities.CheckIn> eventCheckIns = logisticsDbContext.CheckIns!
            .AsNoTracking()
            .Where(c => c.EventId == eventId);

        // Asistentes ingresados = participantes únicos con al menos un check-in.
        int checkedInCount = await eventCheckIns
            .Select(c => c.ParticipantId)
            .Distinct()
            .CountAsync(cancellationToken);

        // Raciones consumidas = check-ins asociados a una configuración de ración.
        int rationsConsumed = await eventCheckIns
            .CountAsync(c => c.RationConfigId != null, cancellationToken);

        return new EventMetricsResponse
        {
            TotalCapacity = totalCapacity,
            CheckedInCount = checkedInCount,
            RationsConsumed = rationsConsumed,
        };
    }
}
