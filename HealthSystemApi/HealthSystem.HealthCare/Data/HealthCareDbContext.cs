using HealthSystem.HealthCare.Data.Models;
using HealthSystem.HealthCare.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.HealthCare.Data
{
    public class HealthCareDbContext : DbContext
    {
        public HealthCareDbContext(DbContextOptions<HealthCareDbContext> options) : base(options)
        {
        }

        public DbSet<Medication> Medications { get; set; }
        public DbSet<MedicationSchedule> MedicationSchedules { get; set; }
        public DbSet<HealthIssue> HealthIssues { get; set; }
        public DbSet<Log> Logs { get; set; }

    }
}
