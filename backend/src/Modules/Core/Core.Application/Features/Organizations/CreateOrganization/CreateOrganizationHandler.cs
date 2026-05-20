using Core.Domain.Entities;
using Core.Domain.Repositories;
using MediatR;

namespace Core.Application.Features.Organizations.CreateOrganization;

public class CreateOrganizationHandler(IOrganizationRepository repository) : IRequestHandler<CreateOrganizationCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        Organization organization = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            QrPaymentImageUrl = request.QrPaymentImageUrl,
            CreatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(organization, cancellationToken);

        return organization.Id;
    }
}