using HealthSystem.Problems.Data;
using HealthSystem.Problems.Services.ProblemService;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Problems.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="IApplicationBuilder"/> to initialize the application.
    /// </summary>
    public static class ApplicationBuilder
    {
        /// <summary>
        /// Initializes the application by migrating the database and seeding symptoms.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance used to configure the application.</param>
        public static async void Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(app);
            await SeedSymptoms(services.GetRequiredService<IProblemService>());
        }

        /// <summary>
        /// Applies any pending database migrations.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance used to configure the application.</param>
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

        /// <summary>
        /// Seeds the database with symptom data using the problem service.
        /// </summary>
        /// <param name="problemService">The service used to add symptoms to the database.</param>
        /// <returns>A task representing the asynchronous seed operation.</returns>
        private async static Task SeedSymptoms(IProblemService problemService)
        {
            await problemService.AddSymptomsAsync();
        }
    }
}
