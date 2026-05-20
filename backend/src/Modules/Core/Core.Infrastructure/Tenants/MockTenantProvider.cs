using Core.Application.Tenants;

namespace Core.Infrastructure.Tenants;

public class MockOrganizationProvider: IOrganizationProvider
{
    public Guid GetTenantId() => Guid.Parse("11111111-1111-1111-1111-111111111111");
}