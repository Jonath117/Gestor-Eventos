using Core.Application.Abstractions;

using Microsoft.AspNetCore.Hosting;

namespace Core.Infrastructure.Storage;

/// <summary>
/// Persiste imágenes en <c>wwwroot/{folder}</c> y devuelve la ruta relativa
/// servible vía UseStaticFiles (MVP, almacenamiento local).
/// </summary>
public class LocalImageStorageService(IWebHostEnvironment environment) : IImageStorageService
{
    public async Task<string?> SaveImageAsync(
        string? base64Content,
        string folder,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(base64Content))
        {
            return null;
        }

        string webRoot = environment.WebRootPath
                         ?? Path.Combine(environment.ContentRootPath, "wwwroot");

        string targetDir = Path.Combine(webRoot, folder);
        Directory.CreateDirectory(targetDir);

        // El payload puede venir como data URI ("data:image/png;base64,....").
        string payload = base64Content.Contains(',')
            ? base64Content[(base64Content.IndexOf(',') + 1)..]
            : base64Content;

        byte[] bytes = Convert.FromBase64String(payload);

        string fileName = $"{Guid.NewGuid()}.png";
        await File.WriteAllBytesAsync(
            Path.Combine(targetDir, fileName),
            bytes,
            cancellationToken);

        return $"/{folder}/{fileName}";
    }
}
