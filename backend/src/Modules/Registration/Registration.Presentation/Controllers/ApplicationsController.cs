using Microsoft.AspNetCore.Mvc;
using Registration.Application.DTOs.Requests;
using Registration.Application.Services;

namespace Registration.Presentation.Controllers;

[ApiController]
[Route("api/applications")]
public class ApplicationsController : ControllerBase
{
    private readonly ApplicationQueryService _queryService;

    public ApplicationsController(ApplicationQueryService queryService)
    {
        _queryService = queryService;
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingApplications([FromQuery] Guid tenantId, [FromQuery] Guid eventId)
    {
        var query = new GetPendingApplicationsQuery
        {
            TenantId = tenantId,
            EventId = eventId
        };

        var applications = await _queryService.GetPendingApplicationsAsync(query);

        return Ok(new { applications });
    }
}
