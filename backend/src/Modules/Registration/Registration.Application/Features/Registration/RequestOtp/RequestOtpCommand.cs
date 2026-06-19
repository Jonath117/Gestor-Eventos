using MediatR;

namespace Registration.Application.Features.Registration.RequestOtp;

public record RequestOtpCommand(Guid EventId, string Email, string FullName) : IRequest;