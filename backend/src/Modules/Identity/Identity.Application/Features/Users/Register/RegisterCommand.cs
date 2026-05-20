using MediatR;

namespace Identity.Application.Features.Users.Register;

public record RegisterCommand(string Email, string Password, string Role, string TenantId) : IRequest<RegisterResponse>;
