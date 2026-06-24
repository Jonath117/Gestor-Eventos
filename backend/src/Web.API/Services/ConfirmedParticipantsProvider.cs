using Logistics.Application.Services;

using Microsoft.EntityFrameworkCore;

using Registration.Application.Interfaces;
using Registration.Domain.Enums;

namespace Web.API.Services;

/// <summary>
/// Implementación de <see cref="IConfirmedParticipantsProvider"/> en la capa de
/// composición. Consulta las órdenes confirmadas del módulo Registration y expone
/// sus participantes a Logistics (métricas, validación de QR, listado), sin
/// acoplar ambos módulos.
/// </summary>
public class ConfirmedParticipantsProvider(IRegistrationDbContext dbContext)
    : IConfirmedParticipantsProvider
{
    private IQueryable<Registration.Domain.Entities.Participant> ConfirmedParticipantsQuery(Guid eventId) =>
        dbContext.Orders!
            .AsNoTracking()
            .Where(o => o.EventId == eventId && o.Status == OrderStatus.Confirmed)
            .SelectMany(o => o.Participants);

    public async Task<int> CountConfirmedAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await ConfirmedParticipantsQuery(eventId).CountAsync(cancellationToken);
    }

    public async Task<ConfirmedParticipantDto?> FindConfirmedAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders!
            .AsNoTracking()
            .Where(o => o.EventId == eventId && o.Status == OrderStatus.Confirmed)
            .SelectMany(o => o.Participants, (o, p) => new { o.OrganizationId, p })
            .Where(x => x.p.Id == participantId)
            .Select(x => new ConfirmedParticipantDto(x.p.Id, x.p.FullName, x.OrganizationId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ConfirmedParticipantDto>> GetConfirmedAsync(
        Guid eventId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders!
            .AsNoTracking()
            .Where(o => o.EventId == eventId && o.Status == OrderStatus.Confirmed)
            .SelectMany(o => o.Participants, (o, p) => new ConfirmedParticipantDto(p.Id, p.FullName, o.OrganizationId))
            .ToListAsync(cancellationToken);
    }
}
