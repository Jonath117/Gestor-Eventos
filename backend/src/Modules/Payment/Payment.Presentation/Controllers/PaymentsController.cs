using Microsoft.AspNetCore.Mvc;
using Payment.Application.Abstractions;
using Payment.Application.DTOs.Requests;

namespace Payment.Presentation.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IReceiptService _receiptService;

    public PaymentsController(IReceiptService receiptService)
    {
        _receiptService = receiptService;
    }

    [HttpPost("upload-receipt")]
    public async Task<IActionResult> UploadReceipt([FromBody] UploadReceiptRequest request)
    {
        // Guarda el archivo en el almacenamiento de objetos y persiste el
        // ManualReceipt asociado a la transacción de la orden.
        var receiptUrl = await _receiptService.UploadReceiptAsync(request.ApplicationId, request.FileContentBase64);

        return Ok(new { receiptUrl });
    }
}
