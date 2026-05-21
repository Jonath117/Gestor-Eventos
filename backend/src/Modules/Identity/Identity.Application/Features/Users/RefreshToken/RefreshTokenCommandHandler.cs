using Identity.Application.Interfaces;
using Identity.Domain.Repositories;

using MediatR;

namespace Identity.Application.Features.Users.RefreshToken;

public class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByRefreshTokenAsync(request.Token, cancellationToken);

        if (user == null)
        {
            throw new Exception("Invalid refresh token");
        }

        var refreshToken = user.RefreshTokens.Single(x => x.Token == request.Token);

        if (!refreshToken.IsActive)
        {
            throw new Exception("Refresh token is not active");
        }

        // Rotate token
        var newRefreshTokenString = jwtTokenGenerator.GenerateRefreshToken();
        user.RevokeRefreshToken(request.Token, newRefreshTokenString);
        user.AddRefreshToken(newRefreshTokenString, DateTime.UtcNow.AddDays(7));

        await userRepository.UpdateAsync(user, cancellationToken);

        var orgUser = user.OrganizationUsers.FirstOrDefault();
        var role = orgUser?.Role ?? "User";
        var tenantId = orgUser?.OrganizationId.ToString() ?? "default_tenant";

        var newAccessToken = jwtTokenGenerator.GenerateToken(user.Id, user.Email, role, tenantId);

        return new RefreshTokenResponse(newAccessToken, newRefreshTokenString);
    }
}