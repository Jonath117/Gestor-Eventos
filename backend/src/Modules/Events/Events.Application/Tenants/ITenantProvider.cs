namespace Events.Application.Tenants;

public interface ITenantProvider
{
    Guid GetTenantId();
}