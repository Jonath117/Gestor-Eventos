using System.Text.Json;

using Google.Cloud.PubSub.V1;
using Google.Protobuf;

using Microsoft.Extensions.Logging;

using Registration.Application.Interfaces;
using Registration.Domain.Entities;

namespace Registration.Infrastructure.Messaging;

public class PubSubOtpMessagePublisher(
    PublisherClient publisherClient,
    ILogger<PubSubOtpMessagePublisher> logger) : IOtpMessagePublisher
{
    public async Task PublishOtpRequestAsync(OtpRequest otpRequest, CancellationToken cancellationToken = default)
    {
        var payload = new
        {
            requestId = otpRequest.Id.ToString(),
            userId = otpRequest.UserId,
            tenantId = otpRequest.TenantId,
            email = otpRequest.UserId
        };

        var jsonPayload = JsonSerializer.Serialize(payload);

        var message = new PubsubMessage
        {
            Data = ByteString.CopyFromUtf8(jsonPayload)
        };

        var messageId = await publisherClient.PublishAsync(message);

        logger.LogInformation(
            "Mensaje OTP publicado en Pub/Sub. MessageId: {MessageId}, RequestId: {RequestId}, Email: {Email}",
            messageId,
            otpRequest.Id,
            otpRequest.UserId);
    }
}