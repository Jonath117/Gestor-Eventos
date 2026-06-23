namespace Registration.Application.DTOs.Requests;

public class GetPendingApplicationsQuery
{
    public Guid TenantId { get; set; }
    public Guid EventId { get; set; }
}
