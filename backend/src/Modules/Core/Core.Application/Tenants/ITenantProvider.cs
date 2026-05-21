namespace Core.Application.Tenants;

public interface ITenantProvider
{
    Guid GetTenantId();
    Guid GetCurrentUserId();
}