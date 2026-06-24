using Microsoft.EntityFrameworkCore;

using Payment.Application.Abstractions;
using Payment.Infrastructure.Database;

namespace Payment.Infrastructure.Storage;

/// <summary>
/// Implementación de <see cref="IReceiptReader"/> sobre <see cref="PaymentDbContext"/>:
/// une <c>Transaction</c> (por <c>OrderId</c>) con sus <c>ManualReceipt</c> y devuelve,
/// por orden, la URL del comprobante subido más recientemente.
/// </summary>
public class ReceiptReader(PaymentDbContext dbContext) : IReceiptReader
{
    public async Task<IReadOnlyDictionary<Guid, string>> GetLatestReceiptUrlsAsync(
        IReadOnlyCollection<Guid> orderIds,
        CancellationToken cancellationToken = default)
    {
        if (dbContext.Transactions is null || dbContext.ManualReceipts is null || orderIds.Count == 0)
        {
            return new Dictionary<Guid, string>();
        }

        var rows = await dbContext.Transactions
            .Where(t => orderIds.Contains(t.OrderId))
            .SelectMany(
                t => t.ManualReceipts,
                (t, receipt) => new { t.OrderId, receipt.FileUrl, receipt.UploadedAt })
            .ToListAsync(cancellationToken);

        return rows
            .GroupBy(r => r.OrderId)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(r => r.UploadedAt).First().FileUrl);
    }
}
