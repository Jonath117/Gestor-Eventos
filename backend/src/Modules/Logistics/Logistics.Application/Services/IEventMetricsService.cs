using Logistics.Application.DTOs.Responses;

namespace Logistics.Application.Services;

public interface IEventMetricsService
{
    Task<EventMetricsResponse> GetEventMetricsAsync(
        Guid eventId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lista de participantes confirmados del evento con la cantidad de raciones
    /// consumidas por cada uno.
    /// </summary>
    Task<IReadOnlyList<ConfirmedParticipantResponse>> GetConfirmedParticipantsAsync(
        Guid eventId,
        CancellationToken cancellationToken = default);
}
