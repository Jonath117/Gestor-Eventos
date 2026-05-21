using MediatR;

namespace Identity.Application.Features.Users.Register;

public record RegisterCommand(string Email, string Password) : IRequest<RegisterResponse>;