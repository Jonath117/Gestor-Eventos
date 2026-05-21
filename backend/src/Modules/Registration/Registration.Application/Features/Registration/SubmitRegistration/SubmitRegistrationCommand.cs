using MediatR;

namespace Registration.Application.Features.Registration.SubmitRegistration;

public record SubmitRegistrationCommand(
    Guid EventId,
    string Email,
    string FullName,
    string? Phone) : IRequest<Guid>;