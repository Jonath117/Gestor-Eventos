namespace Identity.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string email, string tenantId);
}
