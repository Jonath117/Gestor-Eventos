using Core.Application.Features.Events.CreateEvent;
using Core.Application.Features.Events.GetAllEvents;
using Core.Application.Features.Events.GetEventById;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core.Presentation.Controllers;

[ApiController]
[Route("api/events")]
[Authorize]
public class EventsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command)
    {
        Guid eventId = await mediator.Send(command);
        return CreatedAtAction(nameof(GetEvent), new { id = eventId }, new { Id = eventId });
    }

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        IEnumerable<GetAllEventsResponse> events = await mediator.Send(new GetAllEventsQuery());
        return Ok(events);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        GetEventByIdResponse response = await mediator.Send(new GetEventByIdQuery(id));
        return Ok(response);
    }


}