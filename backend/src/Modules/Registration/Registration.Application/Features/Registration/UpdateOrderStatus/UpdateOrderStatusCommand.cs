using MediatR;
using Registration.Domain.Enums;

namespace Registration.Application.Features.Registration.UpdateOrderStatus;

public record UpdateOrderStatusCommand(Guid OrderId, OrderStatus Status) : IRequest<bool>;
