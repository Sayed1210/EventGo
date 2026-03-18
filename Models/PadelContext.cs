using EventGo.Models;
using Microsoft.EntityFrameworkCore;

namespace EventGo.Models
{
    public class PadelContext:DbContext
    {
        public PadelContext(DbContextOptions<PadelContext> options):base(options)
        {
        }

        public DbSet<PadelSession> PadelSessions { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Place> Places { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Concert> Concerts { get; set; }





    }
}
