namespace Core.Application.Tenants;

public interface IOrganizationProvider
{
    Guid GetTenantId();
}