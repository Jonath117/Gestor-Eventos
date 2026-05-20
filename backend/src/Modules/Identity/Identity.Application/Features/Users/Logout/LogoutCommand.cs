using MediatR;

namespace Identity.Application.Features.Users.Logout;

public record LogoutCommand(string Token) : IRequest;
