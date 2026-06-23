using MediatR;

namespace Core.Application.Features.Events.CreateEvent;

public record CreateEventCommand(
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    int MaxCapacity,
    Guid OrganizationId,
    string? CoverImageBase64 = null,
    string? PaymentQrBase64 = null) : IRequest<Guid>;