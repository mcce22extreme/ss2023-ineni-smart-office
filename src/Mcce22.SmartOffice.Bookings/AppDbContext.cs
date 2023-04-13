using Mcce22.SmartOffice.Bookings.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Bookings
{
    public class AppDbContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
