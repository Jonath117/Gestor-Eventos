using Identity.Application.Interfaces;
using Identity.Domain.Exceptions;
using MediatR;

namespace Identity.Application.Features.Users.Login;

public class LoginCommandHandler(IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, LoginResponse>
{
    public Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Fake authentication logic
        if (request.Email == "admin@demo.com" && request.Password == "admin123")
        {
            var token = jwtTokenGenerator.GenerateToken("admin_id_123", request.Email, "tenant_demo_1");
            return Task.FromResult(new LoginResponse(token));
        }

        throw new InvalidCredentialsException();
    }
}
