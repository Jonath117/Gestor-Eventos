namespace Identity.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string email, string role, string tenantId);
    string GenerateRefreshToken();
}
