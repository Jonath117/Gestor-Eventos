namespace Core.Application.Abstractions;

public interface IImageStorageService
{
    /// <summary>
    /// Guarda una imagen codificada en base64 dentro de la carpeta indicada
    /// y devuelve su ruta relativa servible (ej. /events/{guid}.png).
    /// Devuelve null si el contenido es nulo o vacío.
    /// </summary>
    Task<string?> SaveImageAsync(
        string? base64Content,
        string folder,
        CancellationToken cancellationToken = default);
}
