using Core.Domain.Entities;

namespace Core.Domain.Repositories;

public interface IOrganizationRepository
{
    Task AddAsync(Organization organization, CancellationToken cancellationToken = default);
    Task<Organization?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Organization>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Organization organization, CancellationToken cancellationToken = default);
}