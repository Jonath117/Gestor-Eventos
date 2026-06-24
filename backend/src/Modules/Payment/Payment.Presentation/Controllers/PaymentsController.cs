using Microsoft.AspNetCore.Mvc;
using Payment.Application.Abstractions;
using Payment.Application.DTOs.Requests;

namespace Payment.Presentation.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IAttachmentStorageService _storageService;

    public PaymentsController(IAttachmentStorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost("upload-receipt")]
    public async Task<IActionResult> UploadReceipt([FromBody] UploadReceiptRequest request)
    {
        var receiptUrl = await _storageService.SaveReceiptAsync(request.ApplicationId, request.FileContentBase64);
        
        // MVP: Here we would typically update the Domain Model (e.g., Application/ManualReceipt)
        // using a repository, but for this simulation we just return the URL.
        
        return Ok(new { receiptUrl });
    }
}
