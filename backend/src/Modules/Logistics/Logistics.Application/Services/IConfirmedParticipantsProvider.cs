namespace Logistics.Application.Services;

/// <summary>
/// Datos mínimos de un participante confirmado, expuestos hacia el módulo
/// Logistics sin acoplarlo con Registration.
/// </summary>
public record ConfirmedParticipantDto(Guid ParticipantId, string FullName, Guid OrganizationId);

/// <summary>
/// Provee información de participantes confirmados (órdenes en estado Confirmed)
/// del módulo Registration. Se implementa en la capa de composición (Web.API)
/// para evitar acoplar Logistics con Registration, siguiendo el mismo patrón que
/// <c>IAcceptanceNotifier</c>.
/// </summary>
public interface IConfirmedParticipantsProvider
{
    /// <summary>Cantidad de participantes en órdenes confirmadas del evento.</summary>
    Task<int> CountConfirmedAsync(Guid eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Devuelve el participante confirmado del evento o <c>null</c> si el
    /// participante no pertenece a una orden confirmada de ese evento.
    /// </summary>
    Task<ConfirmedParticipantDto?> FindConfirmedAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken cancellationToken = default);

    /// <summary>Lista completa de participantes confirmados del evento.</summary>
    Task<IReadOnlyList<ConfirmedParticipantDto>> GetConfirmedAsync(
        Guid eventId,
        CancellationToken cancellationToken = default);
}
