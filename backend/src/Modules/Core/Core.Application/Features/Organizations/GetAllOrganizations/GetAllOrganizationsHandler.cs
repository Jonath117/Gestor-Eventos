using Core.Domain.Repositories;

using MediatR;

namespace Core.Application.Features.Organizations.GetAllOrganizations;

public class GetAllOrganizationsHandler(IOrganizationRepository repository) : IRequestHandler<GetAllOrganizationsQuery, IEnumerable<GetAllOrganizationsResponse>>
{
    public async Task<IEnumerable<GetAllOrganizationsResponse>> Handle(GetAllOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var organizations = await repository.GetAllAsync(cancellationToken);

        return organizations.Select(o => new GetAllOrganizationsResponse(o.Id, o.Name, o.QrPaymentImageUrl));
    }
}