using Core.Application.Tenants;

namespace Core.Infrastructure.Tenants;

public class MockTenantProvider : ITenantProvider
{
    // Mock values until JWT is implemented
    public Guid GetTenantId() => Guid.Parse("11111111-1111-1111-1111-111111111111");
    public Guid GetCurrentUserId() => Guid.Parse("22222222-2222-2222-2222-222222222222");
}