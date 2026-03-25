namespace Events.Application.Tenant;

public interface ITenantProvider
{
    Guid GetTenantId();
}