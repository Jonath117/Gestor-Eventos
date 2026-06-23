using Microsoft.EntityFrameworkCore;

using Registration.Application.DTOs.Requests;
using Registration.Application.DTOs.Responses;
using Registration.Application.Interfaces;
using Registration.Domain.Enums;

namespace Registration.Application.Services;

public class ApplicationQueryService(IRegistrationDbContext dbContext)
{
    public async Task<List<PendingApplicationDto>> GetPendingApplicationsAsync(GetPendingApplicationsQuery query)
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
            .ToListAsync();

        return orders
            .Select(o => new PendingApplicationDto
            {
                Id = o.Id,
                ApplicantName = o.Participants.FirstOrDefault()?.FullName ?? o.ContactEmail,
                PaymentStatus = o.Status.ToString(),
                AppliedAt = o.CreatedAt,
            })
            .ToList();
    }
}
