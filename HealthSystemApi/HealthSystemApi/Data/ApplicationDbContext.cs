﻿using HealthSystemApi.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorInfo> DoctorsInfo { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<HealthIssue> HealthIssues { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<SymptomCategory> SymptomCategories { get; set; }
        public DbSet<SymptomSubCategory> SymptomSubCategories { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Problem>()
                   .HasMany(p => p.Symptoms)
                   .WithMany(s => s.Problems)
                   .UsingEntity<Dictionary<string, object>>(
                       "ProblemSymptom",
                       r => r.HasOne<Symptom>().WithMany().HasForeignKey("SymptomId"),
                       l => l.HasOne<Problem>().WithMany().HasForeignKey("ProblemId"),
                       je =>
                       {
                           je.HasKey("ProblemId", "SymptomId");
                           je.ToTable("ProblemSymptoms");
                       });

            base.OnModelCreating(builder);
        }
    }
}
