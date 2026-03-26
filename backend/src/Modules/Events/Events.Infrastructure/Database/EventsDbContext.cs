using Events.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Database;

public class EventsDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Participant> Participants { get; set; }
    
}