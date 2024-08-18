using HealthSystem.Booking.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Booking.Data
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        public DbSet<AppointmentComment> AppointmentComments { get; set; }
        public DbSet<AppointmentPrescription> AppointmentPrescriptions { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<HealthSystem.Booking.Data.Models.Booking> Bookings { get; set; }
    }
}
