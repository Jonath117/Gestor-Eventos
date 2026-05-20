using MediatR;

namespace Core.Application.Features.Organizations.GetOrganizationById;

public record GetOrganizationByIdQuery(Guid Id) : IRequest<GetOrganizationByIdResponse>;

public record GetOrganizationByIdResponse(Guid Id, string Name, string? QrPaymentImageUrl, DateTime CreatedAt);