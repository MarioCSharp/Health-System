using HealthSystem.Results.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Results.Infrastructure
{
    /// <summary>
    /// Contains extension methods for initializing and migrating the database on application startup.
    /// </summary>
    public static class ApplicationBuilder
    {
        /// <summary>
        /// Initializes the application by migrating the database.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        public static async void Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(app);
        }

        /// <summary>
        /// Migrates the database by applying any pending migrations.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        public static void MigrateDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<ResultsDbContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
