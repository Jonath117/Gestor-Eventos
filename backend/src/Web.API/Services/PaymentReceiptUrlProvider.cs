using Payment.Application.Abstractions;

using Registration.Application.Interfaces;

namespace Web.API.Services;

/// <summary>
/// Implementación de <see cref="IReceiptUrlProvider"/> en la capa de composición.
/// Delega en el lector de comprobantes del módulo Payment, sin acoplar el módulo
/// Registration con Payment.
/// </summary>
public class PaymentReceiptUrlProvider(IReceiptReader receiptReader) : IReceiptUrlProvider
{
    public Task<IReadOnlyDictionary<Guid, string>> GetReceiptUrlsByOrderIdsAsync(
        IReadOnlyCollection<Guid> orderIds,
        CancellationToken cancellationToken = default)
        => receiptReader.GetLatestReceiptUrlsAsync(orderIds, cancellationToken);
}
