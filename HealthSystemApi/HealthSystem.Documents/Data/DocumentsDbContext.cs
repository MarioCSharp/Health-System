using HealthSystem.Documents.Data.Models;
using HealthSystem.Documents.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Documents.Data
{
    public class DocumentsDbContext : DbContext
    {
        public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options) : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var encryptionConverter = new EncryptionConverter();
            var byteEncryptionConverter = new ByteArrayEncryptionConverter();
            var dateEncryptionConverter = new DateTimeEncryptionConverter();

            modelBuilder.Entity<Reminder>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasConversion(encryptionConverter);

                entity.Property(e => e.RemindTime)
                    .HasConversion(dateEncryptionConverter);
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(e => e.File)
                    .HasConversion(byteEncryptionConverter);

                entity.Property(e => e.Notes)
                    .HasConversion(encryptionConverter);
            });
        }
    }
}
