using Registration.Domain.Entities;

namespace Registration.Application.Interfaces;

public interface IOtpMessagePublisher
{
    Task PublishOtpRequestAsync(OtpRequest otpRequest, CancellationToken cancellationToken = default);
}