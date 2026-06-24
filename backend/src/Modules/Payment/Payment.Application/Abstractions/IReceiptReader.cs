namespace Payment.Application.Abstractions;

/// <summary>
/// Lectura de comprobantes del módulo de pagos por orden. Permite que otros módulos
/// (vía un adaptador en la capa de composición) obtengan la URL del comprobante sin
/// acoplarse a las entidades de Payment.
/// </summary>
public interface IReceiptReader
{
    /// <summary>
    /// Devuelve un mapa <c>orderId → URL del comprobante más reciente</c> para las
    /// órdenes pedidas. Las órdenes sin comprobante no aparecen en el resultado.
    /// </summary>
    Task<IReadOnlyDictionary<Guid, string>> GetLatestReceiptUrlsAsync(
        IReadOnlyCollection<Guid> orderIds,
        CancellationToken cancellationToken = default);
}
