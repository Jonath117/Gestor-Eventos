using MediatR;

namespace Registration.Application.Features.Registration.VerifyOtp;

public record VerifyOtpCommand(Guid EventId, string Email, string Otp) : IRequest<bool>;