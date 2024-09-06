using HealthSystem.Pharmacy.Data.Models;
using HealthSystem.Pharmacy.Infrastructure;
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
        public DbSet<UserCart> UserCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Pharmacist> Pharmacists { get; set; }
        public DbSet<PharmacyOwner> PharmacyOwners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var encryptionConverter = new EncryptionConverter();

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

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.UserCart)
                .WithMany(uc => uc.CartItems)
                .HasForeignKey(ci => ci.UserCartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Medication)
                .WithMany()
                .HasForeignKey(ci => ci.MedicationId);

            modelBuilder.Entity<UserCart>()
                .HasOne(uc => uc.Pharmacy)
                .WithMany()
                .HasForeignKey(uc => uc.PharmacyId);

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Location)
                      .HasConversion(encryptionConverter);

                entity.Property(e => e.PhoneNumber)
                      .HasConversion(encryptionConverter);

                entity.Property(e => e.Name)
                      .HasConversion(encryptionConverter);
            });
        }
    }
}
