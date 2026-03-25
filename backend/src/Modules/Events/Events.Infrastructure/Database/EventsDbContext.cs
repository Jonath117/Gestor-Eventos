using Events.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Database;

public class EventsDbContext: DbContext
{
    public EventsDbContext(DbContextOptions options) : base(options) {}
    
    public DbSet<Event> Events { get; set; }
    public DbSet<Participant> Participants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}