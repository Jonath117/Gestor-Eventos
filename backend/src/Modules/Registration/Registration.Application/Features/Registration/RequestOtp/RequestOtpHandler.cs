using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Registration.Application.Interfaces;
using Registration.Domain.Entities;

namespace Registration.Application.Features.Registration.RequestOtp;

public class RequestOtpHandler(
    IRegistrationDbContext dbContext,
    IOtpMessagePublisher messagePublisher,
    ILogger<RequestOtpHandler> logger) : IRequestHandler<RequestOtpCommand>
{
    public async Task Handle(RequestOtpCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Recibida solicitud de OTP. Registrando en DB. Evento: {EventId}, Email: {Email}",
            request.EventId,
            request.Email);

        var otpRequest = new OtpRequest
        {
            Id = Guid.NewGuid(),
            UserId = request.Email,
            TenantId = request.EventId.ToString(),
            Code = null, // Será generado y procesado por la función serverless FaaS
            Status = "pendiente",
            CreatedAt = DateTime.UtcNow
        };

        if (dbContext.OtpRequests != null)
        {
            await dbContext.OtpRequests.AddAsync(otpRequest, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        logger.LogInformation(
            "Solicitud de OTP persistida con ID: {Id} en estado PENDIENTE.",
            otpRequest.Id);

        await messagePublisher.PublishOtpRequestAsync(otpRequest, cancellationToken);

        logger.LogInformation(
            "Mensaje de OTP publicado en Pub/Sub para: {Email}",
            otpRequest.UserId);
    }
}