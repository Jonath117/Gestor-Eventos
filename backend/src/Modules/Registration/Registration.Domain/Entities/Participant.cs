namespace Registration.Domain.Entities;

public class Participant
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string QrIdentifier { get; set; } = string.Empty;
}