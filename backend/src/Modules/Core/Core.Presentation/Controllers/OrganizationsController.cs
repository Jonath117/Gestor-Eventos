using Core.Application.Features.Organizations.CreateOrganization;
using Core.Application.Features.Organizations.GetAllOrganizations;
using Core.Application.Features.Organizations.GetOrganizationById;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrganizationsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationCommand command)
    {
        Guid organizationId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetOrganization), new { id = organizationId }, new { Id = organizationId });
    }

    [HttpGet]
    public async Task<IActionResult> GetOrganizations()
    {
        var organizations = await mediator.Send(new GetAllOrganizationsQuery());
        return Ok(organizations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrganization(Guid id)
    {
        var response = await mediator.Send(new GetOrganizationByIdQuery(id));
        return Ok(response);
    }
}