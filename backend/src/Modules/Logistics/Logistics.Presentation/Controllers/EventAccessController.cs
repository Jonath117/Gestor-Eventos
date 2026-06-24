using Logistics.Application.DTOs.Requests;
using Logistics.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Logistics.Presentation.Controllers;

[ApiController]
[Route("api/events/access")]
public class EventAccessController : ControllerBase
{
    private readonly IEventAccessService _eventAccessService;

    public EventAccessController(IEventAccessService eventAccessService)
    {
        _eventAccessService = eventAccessService;
    }

    [HttpPost("simulate-start")]
    public async Task<IActionResult> SimulateStart([FromBody] SimulateEventStartRequest request)
    {
        await _eventAccessService.SimulateEventStartAsync(request);
        return Ok(new { message = "Simulación iniciada. QRs enviados." });
    }

    [HttpPost("validate-qr")]
    public async Task<IActionResult> ValidateQr([FromBody] ValidateQrRequest request, CancellationToken cancellationToken)
    {
        var response = await _eventAccessService.ValidateQrAsync(request, cancellationToken);
        return Ok(response);
    }
}
