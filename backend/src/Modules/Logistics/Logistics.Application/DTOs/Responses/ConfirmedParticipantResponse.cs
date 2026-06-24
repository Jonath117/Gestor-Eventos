namespace Logistics.Application.DTOs.Responses;

/// <summary>
/// Participante confirmado de un evento junto con la cantidad de raciones que ha
/// consumido (check-ins registrados).
/// </summary>
public class ConfirmedParticipantResponse
{
    public Guid ParticipantId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int RationsConsumed { get; set; }
}
