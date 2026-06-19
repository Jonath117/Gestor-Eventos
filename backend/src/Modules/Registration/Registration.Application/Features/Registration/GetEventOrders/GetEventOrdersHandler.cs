using MediatR;
using Microsoft.EntityFrameworkCore;
using Registration.Application.Interfaces;

namespace Registration.Application.Features.Registration.GetEventOrders;

public class GetEventOrdersHandler(IRegistrationDbContext dbContext) 
    : IRequestHandler<GetEventOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetEventOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders!
            .Include(o => o.Participants)
            .Where(o => o.EventId == request.EventId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);

        return orders.Select(o => new OrderDto(
            o.Id,
            o.ContactEmail,
            o.Status,
            o.CreatedAt,
            o.Participants.Select(p => new ParticipantDto(p.Id, p.FullName, p.Phone)).ToList()
        )).ToList();
    }
}
