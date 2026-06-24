using System.Security.Cryptography;
using System.Text;

using Microsoft.EntityFrameworkCore;

using Payment.Application.Abstractions;
using Payment.Domain.Entities;
using Payment.Domain.Enums;
using Payment.Infrastructure.Database;

namespace Payment.Infrastructure.Storage;

/// <summary>
/// Sube el comprobante a MinIO/S3 (vía <see cref="IAttachmentStorageService"/>) y
/// persiste el <see cref="ManualReceipt"/> contra la <see cref="Transaction"/> de la
/// orden (<c>Transaction.OrderId == applicationId</c>). Si la orden todavía no tiene
/// transacción, crea una pendiente mínima para no romper el flujo del MVP.
/// </summary>
public class ReceiptService(
    IAttachmentStorageService attachmentStorage,
    PaymentDbContext dbContext) : IReceiptService
{
    public async Task<string> UploadReceiptAsync(
        Guid applicationId,
        string base64Content,
        CancellationToken cancellationToken = default)
    {
        string fileUrl = await attachmentStorage.SaveReceiptAsync(applicationId, base64Content);

        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return string.Empty;
        }

        Transaction? transaction = await dbContext.Transactions!
            .FirstOrDefaultAsync(t => t.OrderId == applicationId, cancellationToken);

        if (transaction is null)
        {
            // Fallback MVP: la orden aún no tiene transacción de pago asociada.
            // Creamos una pendiente para poder colgar el comprobante. El monto y la
            // organización reales se resuelven cuando exista el flujo de pago completo.
            transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                OrderId = applicationId,
                OrganizationId = Guid.Empty,
                Amount = 0m,
                Status = TransactionStatus.Pending,
                CreatedAt = DateTime.UtcNow,
            };
            dbContext.Transactions!.Add(transaction);
        }

        // Hash estable por (orden + contenido): permite hacer upsert en re-subidas
        // del mismo comprobante sin violar el índice único de FileHash.
        string fileHash = ComputeHash(applicationId, base64Content);

        ManualReceipt? receipt = await dbContext.ManualReceipts!
            .FirstOrDefaultAsync(m => m.TransactionId == transaction.Id, cancellationToken);

        if (receipt is null)
        {
            receipt = new ManualReceipt
            {
                Id = Guid.NewGuid(),
                TransactionId = transaction.Id,
                Transaction = transaction,
                FileUrl = fileUrl,
                FileHash = fileHash,
                MimeType = "image/png",
                UploadedAt = DateTime.UtcNow,
            };
            dbContext.ManualReceipts!.Add(receipt);
        }
        else
        {
            receipt.FileUrl = fileUrl;
            receipt.FileHash = fileHash;
            receipt.MimeType = "image/png";
            receipt.UploadedAt = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return fileUrl;
    }

    private static string ComputeHash(Guid applicationId, string base64Content)
    {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes($"{applicationId}:{base64Content}"));
        return Convert.ToHexString(hash);
    }
}
