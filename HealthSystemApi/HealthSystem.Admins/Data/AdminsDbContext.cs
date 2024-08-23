using HealthSystem.Admins.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Admins.Data
{
    public class AdminsDbContext : DbContext
    {
        public AdminsDbContext(DbContextOptions<AdminsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorInfo> DoctorsInfo { get; set; }
        public DbSet<DoctorRating> DoctorRatings { get; set; }
    }
}
