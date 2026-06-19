using MediatR;

using Microsoft.EntityFrameworkCore;

using Registration.Application.Interfaces;

namespace Registration.Application.Features.Registration.UpdateOrderStatus;

public class UpdateOrderStatusHandler(IRegistrationDbContext dbContext)
    : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders!
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
            return false;

        order.Status = request.Status;
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}