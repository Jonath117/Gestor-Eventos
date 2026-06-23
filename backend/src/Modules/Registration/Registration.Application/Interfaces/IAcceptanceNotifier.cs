namespace Registration.Application.Interfaces;

/// <summary>
/// Notifica a un participante que su inscripción fue aceptada, enviándole por
/// correo el código QR con el que el staff validará su acceso al evento.
/// La implementación concreta vive en la capa de composición (Web.API) para no
/// acoplar el módulo de Registration con el de Logistics.
/// </summary>
public interface IAcceptanceNotifier
{
    Task NotifyAcceptedAsync(
        Guid eventId,
        Guid participantId,
        string contactEmail,
        CancellationToken cancellationToken = default);
}
