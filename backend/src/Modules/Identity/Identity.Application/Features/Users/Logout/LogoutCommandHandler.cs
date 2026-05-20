using Identity.Domain.Repositories;
using MediatR;

namespace Identity.Application.Features.Users.Logout;

public class LogoutCommandHandler(IUserRepository userRepository) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByRefreshTokenAsync(request.Token, cancellationToken);
        
        if (user == null)
        {
            return; // Or throw if you want to be strict
        }

        user.RevokeRefreshToken(request.Token);
        await userRepository.UpdateAsync(user, cancellationToken);
    }
}
