using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Registration.Application.Features.Registration.RequestOtp;
using Registration.Application.Features.Registration.SubmitRegistration;
using Registration.Application.Features.Registration.VerifyOtp;

namespace Registration.Presentation.Controllers;

[ApiController]
[Route("api/registration")]
public class RegistrationController(ISender sender) : ControllerBase
{
    [HttpPost("{eventId}/request-otp")]
    [AllowAnonymous]
    public async Task<IActionResult> RequestOtp(Guid eventId, [FromBody] RequestOtpRequest request)
    {
        await sender.Send(new RequestOtpCommand(eventId, request.Email, request.FullName));
        return Accepted(new { Message = "OTP request accepted. Pending processing." });
    }

    [HttpPost("{eventId}/verify-otp")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyOtp(Guid eventId, [FromBody] VerifyOtpRequest request)
    {
        var isValid = await sender.Send(new VerifyOtpCommand(eventId, request.Email, request.Otp));

        if (!isValid)
        {
            return BadRequest(new { Message = "Invalid or expired OTP." });
        }

        return Ok(new { Message = "OTP verified successfully." });
    }

    [HttpPost("{eventId}/submit")]
    [AllowAnonymous]
    public async Task<IActionResult> SubmitRegistration(Guid eventId, [FromBody] SubmitRegistrationRequest request)
    {
        var orderId = await sender.Send(new SubmitRegistrationCommand(eventId, request.Email, request.FullName, request.Phone));
        return Ok(new { OrderId = orderId });
    }

    [HttpGet("{eventId}/orders")]
    [Authorize]
    public async Task<IActionResult> GetOrders(Guid eventId)
    {
        var orders = await sender.Send(new Registration.Application.Features.Registration.GetEventOrders.GetEventOrdersQuery(eventId));
        return Ok(orders);
    }

    [HttpPatch("orders/{orderId}/status")]
    [Authorize]
    public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] UpdateOrderStatusRequest request)
    {
        var success = await sender.Send(new Registration.Application.Features.Registration.UpdateOrderStatus.UpdateOrderStatusCommand(orderId, request.Status));
        if (!success)
        {
            return NotFound(new { Message = "Order not found." });
        }
        return Ok(new { Message = "Order status updated successfully." });
    }

    [HttpGet("{eventId}/summary")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEventSummary(Guid eventId)
    {
        var summary = await sender.Send(new Registration.Application.Features.Registration.GetEventSummary.GetEventSummaryQuery(eventId));
        return Ok(summary);
    }
}

public record UpdateOrderStatusRequest(Registration.Domain.Enums.OrderStatus Status);

public record RequestOtpRequest(string Email, string FullName);
public record VerifyOtpRequest(string Email, string Otp);
public record SubmitRegistrationRequest(string Email, string FullName, string? Phone);