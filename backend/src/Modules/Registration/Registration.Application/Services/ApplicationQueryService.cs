using Microsoft.EntityFrameworkCore;

using Registration.Application.DTOs.Requests;
using Registration.Application.DTOs.Responses;
using Registration.Application.Interfaces;
using Registration.Domain.Enums;

namespace Registration.Application.Services;

public class ApplicationQueryService(
    IRegistrationDbContext dbContext,
    IReceiptUrlProvider receiptUrlProvider)
{
    public async Task<List<PendingApplicationDto>> GetPendingApplicationsAsync(
        GetPendingApplicationsQuery query,
        CancellationToken cancellationToken = default)
    {
        if (dbContext.Orders == null)
        {
            return [];
        }

        // Listamos las inscripciones del evento que están pendientes de
        // validación por el staff (las que aún no fueron aceptadas/rechazadas).
        // NOTA: se filtra por EventId (único). La isolación por tenant queda
        // pendiente hasta que el submit resuelva el OrganizationId real del evento.
        var orders = await dbContext.Orders
            .Include(o => o.Participants)
            .Where(o => o.EventId == query.EventId && o.Status == OrderStatus.PaymentPending)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

        // Resolvemos la URL del comprobante (módulo Payment) en un solo lote.
        var orderIds = orders.Select(o => o.Id).ToList();
        var receiptUrls = await receiptUrlProvider.GetReceiptUrlsByOrderIdsAsync(orderIds, cancellationToken);

        return orders
            .Select(o => new PendingApplicationDto
            {
                Id = o.Id,
                ApplicantName = o.Participants.FirstOrDefault()?.FullName ?? o.ContactEmail,
                PaymentStatus = o.Status.ToString(),
                AppliedAt = o.CreatedAt,
                ReceiptFileUrl = receiptUrls.TryGetValue(o.Id, out var url) ? url : null,
            })
            .ToList();
    }
}
