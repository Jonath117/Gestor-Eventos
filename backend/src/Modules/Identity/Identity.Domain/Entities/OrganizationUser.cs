namespace Identity.Domain.Entities;

public class OrganizationUser
{
    public Guid OrganizationId { get; private set; }
    public Guid UserId { get; private set; }
    public string Role { get; private set; } = null!;
    public DateTime JoinedAt { get; private set; }

    private OrganizationUser() { } // EF Core

    public OrganizationUser(Guid organizationId, Guid userId, string role)
    {
        OrganizationId = organizationId;
        UserId = userId;
        Role = role;
        JoinedAt = DateTime.UtcNow;
    }
}