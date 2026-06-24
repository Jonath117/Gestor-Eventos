namespace Logistics.Application.DTOs.Responses;

public class QrValidationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    /// <summary>Participante asociado al QR escaneado (si se pudo identificar).</summary>
    public Guid? ParticipantId { get; set; }
    public string? ParticipantName { get; set; }

    /// <summary>Total de raciones consumidas por el participante tras este escaneo.</summary>
    public int RationsConsumed { get; set; }
}
