using Logistics.Application.DTOs.Requests;
using Logistics.Application.DTOs.Responses;

namespace Logistics.Application.Services;

public interface IEventAccessService
{
    /// <summary>
    /// Genera y envía por correo el QR de cada participante indicado (escenario de prueba).
    /// </summary>
    Task SimulateEventStartAsync(SimulateEventStartRequest request);

    /// <summary>
    /// Valida el QR escaneado: identifica al participante, verifica que esté
    /// confirmado y registra el consumo de una ración (respetando el límite).
    /// </summary>
    Task<QrValidationResponse> ValidateQrAsync(
        ValidateQrRequest request,
        CancellationToken cancellationToken = default);
}
