using MediatR;

using Registration.Domain.Enums;

namespace Registration.Application.Features.Registration.GetEventOrders;

public record ParticipantDto(Guid Id, string FullName, string? Phone);

public record OrderDto(
    Guid Id,
    string ContactEmail,
    OrderStatus Status,
    DateTime CreatedAt,
    List<ParticipantDto> Participants
);

public record GetEventOrdersQuery(Guid EventId) : IRequest<List<OrderDto>>;