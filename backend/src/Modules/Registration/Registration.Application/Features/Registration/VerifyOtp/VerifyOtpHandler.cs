using MediatR;

using Microsoft.Extensions.Caching.Memory;

namespace Registration.Application.Features.Registration.VerifyOtp;

public class VerifyOtpHandler(IMemoryCache cache) : IRequestHandler<VerifyOtpCommand, bool>
{
    public Task<bool> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"OTP_{request.EventId}_{request.Email}";

        if (cache.TryGetValue(cacheKey, out string? cachedOtp) && cachedOtp == request.Otp)
        {
            // Eliminar el OTP usado
            cache.Remove(cacheKey);

            // Marcar como verificado para permitir la inscripción final (válido por 5 minutos)
            var verifiedKey = $"Verified_{request.EventId}_{request.Email}";
            cache.Set(verifiedKey, true, TimeSpan.FromMinutes(5));

            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}