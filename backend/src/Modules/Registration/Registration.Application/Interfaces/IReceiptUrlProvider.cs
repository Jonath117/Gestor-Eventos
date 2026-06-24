namespace Registration.Application.Interfaces;

/// <summary>
/// Provee la URL del comprobante de pago asociado a una inscripción (orden), sin
/// acoplar el módulo Registration con Payment. La implementación concreta vive en
/// la capa de composición (Web.API) y delega en el módulo de pagos.
/// </summary>
public interface IReceiptUrlProvider
{
    /// <summary>
    /// Devuelve un mapa <c>orderId → URL del comprobante</c> para las órdenes pedidas.
    /// Las órdenes sin comprobante no aparecen en el resultado.
    /// </summary>
    Task<IReadOnlyDictionary<Guid, string>> GetReceiptUrlsByOrderIdsAsync(
        IReadOnlyCollection<Guid> orderIds,
        CancellationToken cancellationToken = default);
}
