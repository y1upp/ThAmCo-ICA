using Microsoft.EntityFrameworkCore;

namespace ThAmCo.Events.Data
{
    // DbContext class for interacting with the database
    public class EventsDbContext : DbContext
    {
        // Constructor that takes DbContextOptions as a parameter
        public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options)
        {
        }

        // DbSet properties representing tables in the database

        // Guests table
        public DbSet<Guest> Guests { get; set; }

        // GuestBookings table
        public DbSet<GuestBooking> GuestBookings { get; set; }

        // Staff table
        public DbSet<Staff> Staff { get; set; }

        // Staffings table
        public DbSet<Staffing> Staffings { get; set; }

        // Events table
        public DbSet<Event> Events { get; set; }

        // EventTypes table
        public DbSet<EventType> EventTypes { get; set; }

        // Reservation table
        public DbSet<Reservation> Reservation { get; set; }

        // Staffs table 
        public DbSet<Staff> Staffs { get; set; }

        // Override method for configuring the model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call the base class method
            base.OnModelCreating(modelBuilder);

            // Seed data for the EventType table
            modelBuilder.Entity<EventType>().HasData(
                new EventType { EventTypeId = 1, Name = "Conference" },
                new EventType { EventTypeId = 2, Name = "Workshop" },
                new EventType { EventTypeId = 3, Name = "Music Festival" },
                new EventType { EventTypeId = 4, Name = "Job Showcase" },
                new EventType { EventTypeId = 5, Name = "University showcase" }
            );
        }
    }
}