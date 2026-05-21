using MediatR;

namespace Core.Application.Features.Organizations.GetAllOrganizations;

public record GetAllOrganizationsQuery() : IRequest<IEnumerable<GetAllOrganizationsResponse>>;

public record GetAllOrganizationsResponse(Guid Id, string Name, string? QrPaymentImageUrl);