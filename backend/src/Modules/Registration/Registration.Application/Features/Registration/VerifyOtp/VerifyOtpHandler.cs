using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Registration.Application.Interfaces;

namespace Registration.Application.Features.Registration.VerifyOtp;

public class VerifyOtpHandler(
    IRegistrationDbContext dbContext,
    IMemoryCache cache) : IRequestHandler<VerifyOtpCommand, bool>
{
    public async Task<bool> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.OtpRequests == null)
        {
            return false;
        }

        // Buscar la solicitud de OTP más reciente que coincida con los criterios
        var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
        var otpRecord = await dbContext.OtpRequests
            .FirstOrDefaultAsync(o =>
                o.UserId == request.Email &&
                o.TenantId == request.EventId.ToString() &&
                o.Code == request.Otp &&
                o.Status == "procesado" &&
                o.CreatedAt >= fiveMinutesAgo,
                cancellationToken);

        if (otpRecord != null)
        {
            // Marcar el registro como verificado/usado en la DB para evitar reutilización
            otpRecord.Status = "verificado";
            otpRecord.ProcessedAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);

            // Guardar en el cache en memoria que este correo está verificado para completar la inscripción (válido por 5 minutos)
            var verifiedKey = $"Verified_{request.EventId}_{request.Email}";
            cache.Set(verifiedKey, true, TimeSpan.FromMinutes(5));

            return true;
        }

        return false;
    }
}