namespace Core.Domain.Entities;

public class Event
{
    public Guid Id { get; private set; }
    public Guid OrganizationId { get; private set; }
    public Organization Organization { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int MaxCapacity { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Event() { }

    public static Event Create(
        string name,
        DateTime startDate,
        DateTime endDate,
        int maxCapacity,
        Guid organizationId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacio", nameof(name));

        if (endDate <= startDate)
            throw new ArgumentException("La fecha fin debe ser despues de la fecha inicio");

        if (maxCapacity <= 0)
            throw new ArgumentException("La capacidad maxima debe ser mayor a cero");

        if (organizationId == Guid.Empty)
            throw new ArgumentException("El id de la organizacion es requerido", nameof(organizationId));

        return new Event
        {
            Id = Guid.NewGuid(),
            OrganizationId = organizationId,
            Name = name,
            StartDate = startDate,
            EndDate = endDate,
            MaxCapacity = maxCapacity,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateDetails(string name, DateTime startDate, DateTime endDate, int maxCapacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Event name cannot be empty.", nameof(name));

        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date.");

        if (maxCapacity <= 0)
            throw new ArgumentException("Max capacity must be greater than zero.");

        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        MaxCapacity = maxCapacity;
    }
}