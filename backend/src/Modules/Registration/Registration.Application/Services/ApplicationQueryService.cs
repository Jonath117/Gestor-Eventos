using Registration.Application.DTOs.Requests;
using Registration.Application.DTOs.Responses;

namespace Registration.Application.Services;

public class ApplicationQueryService
{
    public Task<List<PendingApplicationDto>> GetPendingApplicationsAsync(GetPendingApplicationsQuery query)
    {
        // Dummy implementation returning simulated data.
        var dummyData = new List<PendingApplicationDto>
        {
            new PendingApplicationDto
            {
                Id = Guid.NewGuid(),
                ApplicantName = "Juan Perez",
                PaymentStatus = "PendingValidation",
                AppliedAt = DateTime.UtcNow.AddHours(-2)
            },
            new PendingApplicationDto
            {
                Id = Guid.NewGuid(),
                ApplicantName = "Maria Gomez",
                PaymentStatus = "PendingValidation",
                AppliedAt = DateTime.UtcNow.AddHours(-5)
            }
        };

        return Task.FromResult(dummyData);
    }
}
