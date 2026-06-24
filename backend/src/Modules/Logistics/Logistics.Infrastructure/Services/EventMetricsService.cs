using Core.Application.Abstractions;

using Logistics.Application.DTOs.Responses;
using Logistics.Application.Services;
using Logistics.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Services;

/// <summary>
/// Calcula las métricas operativas de un evento cruzando los check-ins y
/// raciones registrados en Logistics con la capacidad máxima definida en Core y
/// los participantes confirmados del módulo Registration.
/// </summary>
public class EventMetricsService(
    LogisticsDbContext logisticsDbContext,
    ICoreDbContext coreDbContext,
    IConfirmedParticipantsProvider confirmedParticipantsProvider) : IEventMetricsService
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

        // Confirmados = participantes en órdenes aceptadas (módulo Registration).
        // Base de los cupos disponibles (capacidad − confirmados).
        int confirmedCount = await confirmedParticipantsProvider
            .CountConfirmedAsync(eventId, cancellationToken);

        return new EventMetricsResponse
        {
            TotalCapacity = totalCapacity,
            CheckedInCount = checkedInCount,
            RationsConsumed = rationsConsumed,
            ConfirmedCount = confirmedCount,
        };
    }

    public async Task<IReadOnlyList<ConfirmedParticipantResponse>> GetConfirmedParticipantsAsync(
        Guid eventId,
        CancellationToken cancellationToken = default)
    {
        var confirmed = await confirmedParticipantsProvider
            .GetConfirmedAsync(eventId, cancellationToken);

        if (confirmed.Count == 0)
            return [];

        // Raciones consumidas por participante = check-ins del evento agrupados.
        Dictionary<Guid, int> rationsByParticipant = await logisticsDbContext.CheckIns!
            .AsNoTracking()
            .Where(c => c.EventId == eventId && c.RationConfigId != null)
            .GroupBy(c => c.ParticipantId)
            .Select(g => new { ParticipantId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.ParticipantId, x => x.Count, cancellationToken);

        return confirmed
            .Select(p => new ConfirmedParticipantResponse
            {
                ParticipantId = p.ParticipantId,
                FullName = p.FullName,
                RationsConsumed = rationsByParticipant.GetValueOrDefault(p.ParticipantId, 0),
            })
            .ToList();
    }
}
