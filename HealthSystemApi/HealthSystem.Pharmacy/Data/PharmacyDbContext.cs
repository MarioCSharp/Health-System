using HealthSystem.Pharmacy.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Pharmacy.Data
{
    public class PharmacyDbContext : DbContext
    {
        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Pharmacy> Pharmacies { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderMedication> OrderMedications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderMedication>()
                .HasKey(om => new { om.OrderId, om.MedicationId });

            modelBuilder.Entity<OrderMedication>()
                .HasOne(om => om.Order)
                .WithMany(o => o.OrderMedications)
                .HasForeignKey(om => om.OrderId);

            modelBuilder.Entity<OrderMedication>()
                .HasOne(om => om.Medication)
                .WithMany()
                .HasForeignKey(om => om.MedicationId);
        }
    }
}
