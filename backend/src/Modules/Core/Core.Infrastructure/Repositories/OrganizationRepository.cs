using Core.Domain.Entities;
using Core.Domain.Repositories;
using Core.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories;

public class OrganizationRepository(CoreDbContext context) : IOrganizationRepository
{
    public async Task AddAsync(Organization organization, CancellationToken cancellationToken = default)
    {
        await context.Organizations.AddAsync(organization, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Organizations
            .Include(o => o.OrganizationUsers)
            .Include(o => o.Events)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Organization>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Organizations.ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Organization organization, CancellationToken cancellationToken = default)
    {
        context.Organizations.Update(organization);
        await context.SaveChangesAsync(cancellationToken);
    }
}