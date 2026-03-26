using Events.Application.Features.Events.RegisterParticipant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Presentation.Controllers;

[ApiController]
[Route("api/events/{eventId:guid}/participants")]
public class RegistrationController(ISender sender) : ControllerBase
{
    // DTO específico para la petición HTTP que excluye el EventId de la URL
    public record RegisterParticipantRequest(string FullName, string Email);

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterParticipant(Guid eventId, [FromBody] RegisterParticipantRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new RegisterParticipantCommand(eventId, request.FullName, request.Email);
            var response = await sender.Send(command, cancellationToken);
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            return ex.GetType().Name switch
            {
                "EventNotFoundException" => NotFound(new { error = ex.Message }),
                "EventIsPastException" or "EventIsFullException" => BadRequest(new { error = ex.Message }),
                _ => StatusCode(500, new { error = "An unexpected error occurred." })
            };
        }
    }
}
