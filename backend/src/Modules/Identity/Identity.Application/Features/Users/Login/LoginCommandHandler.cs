using Identity.Application.Interfaces;
using Identity.Domain.Exceptions;
using Identity.Domain.Repositories;

using MediatR;

namespace Identity.Application.Features.Users.Login;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        // Determine role and tenant (Logic: Use first available organization or handle if none)
        var orgUser = user.OrganizationUsers.FirstOrDefault();
        var role = orgUser?.Role ?? "User";
        var tenantId = orgUser?.OrganizationId.ToString() ?? "default_tenant";

        var accessToken = jwtTokenGenerator.GenerateToken(user.Id, user.Email, role, tenantId);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        user.AddRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
        await userRepository.UpdateAsync(user, cancellationToken);

        return new LoginResponse(accessToken, refreshToken);
    }
}