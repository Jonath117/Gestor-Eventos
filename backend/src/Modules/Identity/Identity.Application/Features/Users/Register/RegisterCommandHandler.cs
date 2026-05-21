using Identity.Domain.Entities;
using Identity.Domain.Repositories;

using MediatR;

namespace Identity.Application.Features.Users.Register;

public class RegisterCommandHandler(IUserRepository userRepository) : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if user already exists
        var existingUser = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new Exception("User already exists");
        }

        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Create user
        var user = User.Create(request.Email, passwordHash);

        // Save user
        await userRepository.AddAsync(user, cancellationToken);

        return new RegisterResponse(user.Id, user.Email);
    }
}