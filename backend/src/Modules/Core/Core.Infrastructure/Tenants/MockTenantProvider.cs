using System.Security.Claims;

using Core.Application.Tenants;

using Microsoft.AspNetCore.Http;

namespace Core.Infrastructure.Tenants;

public class MockTenantProvider(IHttpContextAccessor httpContextAccessor) : ITenantProvider
{
    // Mock values until full tenant resolution is implemented
    public Guid GetTenantId() => Guid.Parse("11111111-1111-1111-1111-111111111111");

    public Guid GetCurrentUserId()
    {
        var userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            // Fallback for local development if no token is present
            return Guid.Parse("49528646-5ae1-46cd-b871-4476db7a145a");
        }

        return Guid.Parse(userIdClaim);
    }
}