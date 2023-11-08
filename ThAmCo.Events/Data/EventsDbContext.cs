using Microsoft.EntityFrameworkCore;

namespace ThAmCo.Events.Data
{
    public class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options)
        {
        }

        public DbSet<Guest> Guests { get; set; }
        public DbSet<GuestBooking> GuestBookings { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Staffing> Staffings { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}