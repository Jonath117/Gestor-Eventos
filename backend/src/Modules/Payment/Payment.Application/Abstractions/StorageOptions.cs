namespace Payment.Application.Abstractions;

/// <summary>
/// Configuración del almacenamiento de objetos (MinIO / S3) para el módulo de pagos.
/// Se enlaza desde la sección "Storage". <see cref="PublicBaseUrl"/> es el host que
/// queda embebido en la URL del comprobante devuelta al cliente (debe ser alcanzable
/// desde el navegador o el dispositivo móvil).
/// </summary>
public class StorageOptions
{
    public const string SectionName = "Storage";

    public string ServiceUrl { get; set; } = "http://localhost:9000";
    public string PublicBaseUrl { get; set; } = "http://localhost:9000";
    public string BucketName { get; set; } = "gestor-eventos";
}
