namespace Payment.Application.DTOs.Requests;

public class UploadReceiptRequest
{
    public Guid ApplicationId { get; set; }
    public string FileContentBase64 { get; set; } = string.Empty;
}
