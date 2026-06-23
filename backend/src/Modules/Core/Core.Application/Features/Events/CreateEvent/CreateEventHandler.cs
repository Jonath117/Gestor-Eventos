using Core.Application.Abstractions;
using Core.Application.Tenants;
using Core.Domain.Entities;
using Core.Domain.Repositories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.Events.CreateEvent;

public class CreateEventHandler(
        ICoreDbContext context,
        ITenantProvider tenantProvider,
        IImageStorageService imageStorage) : IRequestHandler<CreateEventCommand, Guid>
{
    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        Guid organizationId = request.OrganizationId;
        Guid userId = tenantProvider.GetCurrentUserId();

        // Validar que el usuario pertenece a la organización y es Admin
        bool isAdmin = await context.OrganizationUsers
            .AnyAsync(ou => ou.OrganizationId == organizationId &&
                            ou.UserId == userId &&
                            ou.Role == "Admin",
                      cancellationToken);

        if (!isAdmin)
        {
            throw new UnauthorizedAccessException("Only administrators can create events for this organization.");
        }

        string? coverImageUrl = await imageStorage.SaveImageAsync(
            request.CoverImageBase64, "events", cancellationToken);
        string? paymentQrImageUrl = await imageStorage.SaveImageAsync(
            request.PaymentQrBase64, "payment-qrs", cancellationToken);

        Event newEvent = Event.Create(
            request.Name,
            DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc),
            DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc),
            request.MaxCapacity,
            organizationId,
            coverImageUrl,
            paymentQrImageUrl);

        await context.Events.AddAsync(newEvent, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return newEvent.Id;
    }

}