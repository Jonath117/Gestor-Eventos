using Microsoft.AspNetCore.Mvc;
using Payment.Application.DTOs.Requests;
using Payment.Application.Services;

namespace Payment.Presentation.Controllers;

[ApiController]
[Route("api/payments/links")]
public class PaymentLinksController : ControllerBase
{
    private readonly PaymentLinkGeneratorService _generatorService;

    public PaymentLinksController(PaymentLinkGeneratorService generatorService)
    {
        _generatorService = generatorService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateLink([FromBody] GeneratePaymentLinkRequest request)
    {
        var paymentLink = await _generatorService.GenerateLinkAsync(request.ApplicationId);
        return Ok(new { paymentLink });
    }
}
