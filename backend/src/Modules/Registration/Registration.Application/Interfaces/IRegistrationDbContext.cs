using Microsoft.EntityFrameworkCore;

using Registration.Domain.Entities;

namespace Registration.Application.Interfaces;

public interface IRegistrationDbContext
{
    DbSet<Order>? Orders { get; }
    DbSet<Participant>? Participants { get; }
    DbSet<Code>? Codes { get; }
    DbSet<OtpRequest>? OtpRequests { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}