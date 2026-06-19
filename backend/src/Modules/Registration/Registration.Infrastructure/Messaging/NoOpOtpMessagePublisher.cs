using Microsoft.Extensions.Logging;

using Registration.Application.Interfaces;
using Registration.Domain.Entities;

namespace Registration.Infrastructure.Messaging;

public class NoOpOtpMessagePublisher(
    ILogger<NoOpOtpMessagePublisher> logger) : IOtpMessagePublisher
{
    public Task PublishOtpRequestAsync(OtpRequest otpRequest, CancellationToken cancellationToken = default)
    {
        logger.LogWarning(
            "Pub/Sub no configurado. OTP request {RequestId} para {Email} NO fue publicado. Configure PubSub:ProjectId y PubSub:TopicId.",
            otpRequest.Id,
            otpRequest.UserId);

        return Task.CompletedTask;
    }
}