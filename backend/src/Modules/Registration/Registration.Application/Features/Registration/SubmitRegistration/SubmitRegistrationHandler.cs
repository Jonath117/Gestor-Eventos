using MediatR;

using Microsoft.Extensions.Caching.Memory;

using Registration.Application.Interfaces;
using Registration.Domain.Entities;
using Registration.Domain.Enums;

namespace Registration.Application.Features.Registration.SubmitRegistration;

public class SubmitRegistrationHandler(
    IMemoryCache cache,
    IRegistrationDbContext dbContext) : IRequestHandler<SubmitRegistrationCommand, Guid>
{
    public async Task<Guid> Handle(SubmitRegistrationCommand request, CancellationToken cancellationToken)
    {
        var verifiedKey = $"Verified_{request.EventId}_{request.Email}";

        if (!cache.TryGetValue(verifiedKey, out bool isVerified) || !isVerified)
        {
            throw new Exception("OTP verification required or expired.");
        }

        // En un escenario real, buscaríamos el OrganizationId del evento.
        // Como estamos en un modular monolith, podríamos usar un IEventService o similar.
        // Por ahora, usaremos un Guid fijo o intentaremos obtenerlo si es posible.
        // Para cumplir con la base de datos real, crearemos el registro.

        // TODO: En producción, obtener el OrganizationId real del módulo Core.
        var organizationId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            EventId = request.EventId,
            ContactEmail = request.Email,
            Status = OrderStatus.Confirmed,
            CreatedAt = DateTime.UtcNow
        };

        var participant = new Participant
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            FullName = request.FullName,
            Phone = request.Phone,
            QrIdentifier = $"QR_{order.Id}_{Guid.NewGuid().ToString()[..8]}"
        };

        order.Participants.Add(participant);

        if (dbContext.Orders != null)
        {
            await dbContext.Orders.AddAsync(order, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        // Limpiar el estado de verificado
        cache.Remove(verifiedKey);

        return order.Id;
    }
}