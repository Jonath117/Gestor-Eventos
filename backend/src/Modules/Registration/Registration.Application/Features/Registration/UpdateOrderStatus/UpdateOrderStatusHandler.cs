using MediatR;

using Microsoft.EntityFrameworkCore;

using Registration.Application.Interfaces;
using Registration.Domain.Enums;

namespace Registration.Application.Features.Registration.UpdateOrderStatus;

public class UpdateOrderStatusHandler(
    IRegistrationDbContext dbContext,
    IAcceptanceNotifier acceptanceNotifier)
    : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders!
            .Include(o => o.Participants)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
            return false;

        var wasConfirmed = order.Status == OrderStatus.Confirmed;
        order.Status = request.Status;
        await dbContext.SaveChangesAsync(cancellationToken);

        // Al aceptar (Confirmed) por primera vez, se envía el QR por correo a
        // cada participante; ese QR es el que el staff escanea en el acceso.
        if (request.Status == OrderStatus.Confirmed && !wasConfirmed)
        {
            foreach (var participant in order.Participants)
            {
                await acceptanceNotifier.NotifyAcceptedAsync(
                    order.EventId,
                    participant.Id,
                    order.ContactEmail,
                    cancellationToken);
            }
        }

        return true;
    }
}
