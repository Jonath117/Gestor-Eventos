using Identity.Application.Features.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(ISender sender) : ControllerBase
{
    [HttpPost("login")]
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

            return StatusCode(500, new { error = "An unexpected error occurred." });
        }
    }
}
