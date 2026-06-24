namespace Logistics.Application.DTOs.Responses;

public class EventMetricsResponse
{
    public int TotalCapacity { get; set; }
    public int CheckedInCount { get; set; }
    public int RationsConsumed { get; set; }

    /// <summary>
    /// Participantes en órdenes confirmadas (aceptadas). Base para calcular los
    /// cupos disponibles (capacidad − confirmados).
    /// </summary>
    public int ConfirmedCount { get; set; }
}
