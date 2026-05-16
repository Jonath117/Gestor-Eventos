namespace Core.Domain.Entities;

public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? QrPaymentImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<OrganizationUser> OrganizationUsers { get; set; } = [];
    public List<Event> Events { get; set; } = [];
}