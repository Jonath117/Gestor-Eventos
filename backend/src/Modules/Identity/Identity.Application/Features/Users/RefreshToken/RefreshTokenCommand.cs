using MediatR;

namespace Identity.Application.Features.Users.RefreshToken;

public record RefreshTokenCommand(string Token) : IRequest<RefreshTokenResponse>;
