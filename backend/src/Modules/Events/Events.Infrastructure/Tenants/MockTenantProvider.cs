using Events.Application.Tenant;

namespace Events.Infrastructure.Tenant;

public class MockTenantProvider: ITenantProvider
{
    public Guid GetTenantId() => Guid.Parse("11111111-1111-1111-1111-111111111111");
}