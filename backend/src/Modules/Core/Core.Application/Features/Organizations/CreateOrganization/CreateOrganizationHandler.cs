using Core.Application.Abstractions;
using Core.Application.Tenants;
using Core.Domain.Entities;

using MediatR;

namespace Core.Application.Features.Organizations.CreateOrganization;

public class CreateOrganizationHandler(
    ICoreDbContext context,
    ITenantProvider tenantProvider) : IRequestHandler<CreateOrganizationCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        Guid currentUserId = tenantProvider.GetCurrentUserId();

        Organization organization = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            QrPaymentImageUrl = request.QrPaymentImageUrl,
            CreatedAt = DateTime.UtcNow
        };

        OrganizationUser adminMember = new()
        {
            OrganizationId = organization.Id,
            UserId = currentUserId,
            Role = "Admin",
            JoinedAt = DateTime.UtcNow
        };

        await context.Organizations.AddAsync(organization, cancellationToken);
        await context.OrganizationUsers.AddAsync(adminMember, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return organization.Id;
    }
}