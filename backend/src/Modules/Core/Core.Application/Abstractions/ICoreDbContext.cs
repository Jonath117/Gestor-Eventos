using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Abstractions;

public interface ICoreDbContext
{
    DbSet<Organization> Organizations { get; }
    DbSet<User> Users { get; }
    DbSet<OrganizationUser> OrganizationUsers { get; }
    DbSet<Event> Events { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
