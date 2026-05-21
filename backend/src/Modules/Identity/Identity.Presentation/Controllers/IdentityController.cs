using Identity.Application.Features.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Identity.Presentation.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(ISender sender) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("PublicEndpointsPolicy")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            // Simple global error handling (Ideally should be handled by a global exception handler or ProblemDetails)
            if (ex.GetType().Name == "InvalidCredentialsException")
            {
                return Unauthorized(new { error = ex.Message });
            }

            return StatusCode(500, new { 
                error = "An unexpected error occurred.", 
                details = ex.Message,
                stackTrace = ex.StackTrace 
            });
        }
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        return Ok(new { Message = "You are authenticated!", Claims = claims });
    }
}
