using HealthSystem.Results.Data.Models;
using HealthSystem.Results.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Results.Data
{
    public class ResultsDbContext : DbContext
    {
        public ResultsDbContext(DbContextOptions<ResultsDbContext> options) : base(options)
        {
        }

        public DbSet<LaboratoryResult> LaboratoryResults { get; set; }
        public DbSet<IssuedRecipe> IssuedRecipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var encryptionConverter = new EncryptionConverter();
            var byteArrayEncryptionConverter = new ByteArrayEncryptionConverter();

            modelBuilder.Entity<IssuedRecipe>(entity =>
            {
                entity.Property(e => e.EGN)
                      .HasConversion(encryptionConverter);

                entity.Property(e => e.File)
                      .HasConversion(byteArrayEncryptionConverter);
            });

            modelBuilder.Entity<LaboratoryResult>(entity =>
            {
                entity.Property(e => e.File)
                      .HasConversion(byteArrayEncryptionConverter);

                entity.Property(e => e.UserLogingName)
                      .HasConversion(encryptionConverter);

                entity.Property(e => e.UserLogingPass)
                      .HasConversion(encryptionConverter);
            });
        }
    }
}
