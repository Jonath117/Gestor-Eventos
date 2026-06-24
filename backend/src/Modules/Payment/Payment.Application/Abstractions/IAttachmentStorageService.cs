namespace Payment.Application.Abstractions;

public interface IAttachmentStorageService
{
    Task<string> SaveReceiptAsync(Guid applicationId, string base64Content);
}
