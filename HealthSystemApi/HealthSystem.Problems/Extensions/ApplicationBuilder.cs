using HealthSystem.Problems.Data;
using HealthSystem.Problems.Services.ProblemService;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Problems.Extensions
{
    public static class ApplicationBuilder
    {
        public static async void Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(app);
            await SeedSymptoms(services.GetRequiredService<IProblemService>());
        }

        public static void MigrateDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<ProblemsDbContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
        }

        private async static Task SeedSymptoms(IProblemService problemService)
        {
            await problemService.AddSymptomsAsync();
        }
    }
}
