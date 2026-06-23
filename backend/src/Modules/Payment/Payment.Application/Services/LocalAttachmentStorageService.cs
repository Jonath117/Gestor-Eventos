using Microsoft.AspNetCore.Hosting;

namespace Payment.Application.Services;

/// <summary>
/// Persiste los comprobantes en <c>wwwroot/receipts</c> y devuelve la ruta
/// relativa servible por el host estático (MVP, almacenamiento local).
/// </summary>
public class LocalAttachmentStorageService(IWebHostEnvironment environment)
{
    private const string ReceiptsFolder = "receipts";

    public async Task<string> SaveReceiptAsync(Guid applicationId, string base64Content)
    {
        string webRoot = environment.WebRootPath
                         ?? Path.Combine(environment.ContentRootPath, "wwwroot");

        string receiptsDir = Path.Combine(webRoot, ReceiptsFolder);
        Directory.CreateDirectory(receiptsDir);

        string fileName = $"{applicationId}.png";
        string fullPath = Path.Combine(receiptsDir, fileName);

        // El payload puede venir como data URI ("data:image/png;base64,....").
        string payload = base64Content.Contains(',')
            ? base64Content[(base64Content.IndexOf(',') + 1)..]
            : base64Content;

        byte[] bytes = Convert.FromBase64String(payload);
        await File.WriteAllBytesAsync(fullPath, bytes);

        // Ruta relativa servida por UseStaticFiles (ej. http://host/receipts/{id}.png).
        return $"/{ReceiptsFolder}/{fileName}";
    }
}
