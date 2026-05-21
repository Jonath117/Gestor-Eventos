using MediatR;

namespace Identity.Application.Features.Users.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;