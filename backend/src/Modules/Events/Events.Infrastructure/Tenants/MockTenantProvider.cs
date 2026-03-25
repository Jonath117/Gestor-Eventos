using Events.Application.Tenants;

namespace Events.Infrastructure.Tenants;

public class MockTenantProvider: ITenantProvider
{
    public Guid GetTenantId() => Guid.Parse("11111111-1111-1111-1111-111111111111");
}