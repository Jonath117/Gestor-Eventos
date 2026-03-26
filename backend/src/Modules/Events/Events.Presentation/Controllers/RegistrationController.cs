using Events.Application.Features.Events.RegisterParticipant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;

namespace Events.Presentation.Controllers;

[ApiController]
[Route("api/events/{eventId:guid}/participants")]
public class RegistrationController(ISender sender, ILogger<RegistrationController> logger) : ControllerBase
{
    // DTO específico para la petición HTTP que excluye el EventId de la URL
    public record RegisterParticipantRequest(string FullName, string Email);

    [HttpPost]
    [AllowAnonymous]
    [EnableRateLimiting("PublicEndpointsPolicy")]
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
            // Manejo de excepciones conocidas
            if (ex.GetType().Name == "EventNotFoundException")
                return NotFound(new { error = ex.Message });
                
            if (ex.GetType().Name is "EventIsPastException" or "EventIsFullException")
                return BadRequest(new { error = ex.Message });
                
            // Para cualquier otra excepción, registrarla y devolver un 500 genérico
            logger.LogError(ex, "An unexpected error occurred while registering a participant for event {EventId}", eventId);
            return StatusCode(500, new { error = "An unexpected error occurred." });
        }
    }
}
