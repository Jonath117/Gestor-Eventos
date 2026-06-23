using Logistics.Application.DTOs.Responses;

namespace Logistics.Application.Services;

public interface IEventMetricsService
{
    Task<EventMetricsResponse> GetEventMetricsAsync(
        Guid eventId,
        CancellationToken cancellationToken = default);
}
