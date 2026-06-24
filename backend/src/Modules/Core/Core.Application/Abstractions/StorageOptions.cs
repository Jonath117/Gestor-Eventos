namespace Core.Application.Abstractions;

/// <summary>
/// Configuración del almacenamiento de objetos (MinIO / S3). Se enlaza desde la
/// sección "Storage" de la configuración. <see cref="ServiceUrl"/> es el endpoint
/// interno que usa el SDK para subir objetos; <see cref="PublicBaseUrl"/> es el host
/// que se devuelve al cliente y queda embebido en las URLs (el que debe ser
/// alcanzable desde el navegador o el dispositivo móvil).
/// </summary>
public class StorageOptions
{
    public const string SectionName = "Storage";

    public string ServiceUrl { get; set; } = "http://localhost:9000";
    public string PublicBaseUrl { get; set; } = "http://localhost:9000";
    public string BucketName { get; set; } = "gestor-eventos";
}
