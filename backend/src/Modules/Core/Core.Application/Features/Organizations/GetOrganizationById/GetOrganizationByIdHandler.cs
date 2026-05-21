using Core.Domain.Exceptions;
using Core.Domain.Repositories;

using MediatR;

namespace Core.Application.Features.Organizations.GetOrganizationById;

public class GetOrganizationByIdHandler(IOrganizationRepository repository) : IRequestHandler<GetOrganizationByIdQuery, GetOrganizationByIdResponse>
{
    public async Task<GetOrganizationByIdResponse> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
    {
        var organization = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (organization is null)
        {
            throw new OrganizationNotFoundException(request.Id);
        }

        return new GetOrganizationByIdResponse(
            organization.Id,
            organization.Name,
            organization.QrPaymentImageUrl,
            organization.CreatedAt);
    }
}