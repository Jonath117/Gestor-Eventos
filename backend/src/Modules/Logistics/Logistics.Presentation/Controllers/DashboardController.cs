using Logistics.Application.DTOs.Responses;
using Logistics.Application.Services;

using Microsoft.AspNetCore.Mvc;

namespace Logistics.Presentation.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IEventMetricsService _eventMetricsService;

    public DashboardController(IEventMetricsService eventMetricsService)
    {
        _eventMetricsService = eventMetricsService;
    }

    [HttpGet("metrics/{eventId:guid}")]
    public async Task<IActionResult> GetMetrics(Guid eventId, CancellationToken cancellationToken)
    {
        EventMetricsResponse metrics =
            await _eventMetricsService.GetEventMetricsAsync(eventId, cancellationToken);

        return Ok(metrics);
    }

    [HttpGet("participants/{eventId:guid}")]
    public async Task<IActionResult> GetConfirmedParticipants(Guid eventId, CancellationToken cancellationToken)
    {
        IReadOnlyList<ConfirmedParticipantResponse> participants =
            await _eventMetricsService.GetConfirmedParticipantsAsync(eventId, cancellationToken);

        return Ok(participants);
    }
}
