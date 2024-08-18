using HealthSystem.Problems.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Problems.Data
{
    public class ProblemsDbContext : DbContext
    {
        public ProblemsDbContext(DbContextOptions<ProblemsDbContext> options) : base(options)
        {
        }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<SymptomCategory> SymptomCategories { get; set; }
        public DbSet<SymptomSubCategory> SymptomSubCategories { get; set; }
    }
}
