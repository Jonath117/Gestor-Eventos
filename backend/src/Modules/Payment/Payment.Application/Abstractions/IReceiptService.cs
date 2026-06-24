namespace Payment.Application.Abstractions;

/// <summary>
/// Caso de uso de carga de comprobante: guarda el archivo en el almacenamiento de
/// objetos y persiste el <c>ManualReceipt</c> asociado a la transacción de la orden,
/// devolviendo la URL pública del comprobante.
/// </summary>
public interface IReceiptService
{
    Task<string> UploadReceiptAsync(
        Guid applicationId,
        string base64Content,
        CancellationToken cancellationToken = default);
}
