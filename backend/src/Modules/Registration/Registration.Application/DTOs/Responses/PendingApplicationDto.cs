namespace Registration.Application.DTOs.Responses;

public class PendingApplicationDto
{
    public Guid Id { get; set; }
    public string ApplicantName { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime AppliedAt { get; set; }
}
