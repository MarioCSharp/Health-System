using HealthSystem.Results.Data.Models;
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
    }
}
