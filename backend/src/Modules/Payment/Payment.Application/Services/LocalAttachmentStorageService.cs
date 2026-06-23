namespace Payment.Application.Services;

public class LocalAttachmentStorageService
{
    public Task<string> SaveReceiptAsync(Guid applicationId, string base64Content)
    {
        // Dummy implementation for MVP.
        // Pretends to save the file and returns a dummy URL.
        var dummyUrl = $"https://campeando-storage.dummy/receipts/{applicationId}.png";
        Console.WriteLine($"[Storage Simulation] Saved receipt for application {applicationId} to {dummyUrl}");
        return Task.FromResult(dummyUrl);
    }
}
