using MediatR;

namespace Core.Application.Features.Organizations.CreateOrganization;

public record CreateOrganizationCommand(string Name, string? QrPaymentImageUrl) : IRequest<Guid>;