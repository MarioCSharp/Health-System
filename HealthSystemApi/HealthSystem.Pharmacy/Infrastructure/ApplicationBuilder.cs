using HealthSystem.Pharmacy.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Pharmacy.Infrastructure
{
    /// <summary>
    /// Static class for application initialization tasks, including database migrations.
    /// </summary>
    public static class ApplicationBuilder
    {
        /// <summary>
        /// Initializes the application by applying pending migrations to the database.
        /// </summary>
        /// <param name="app">The application builder instance.</param>
        public static async void Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            // Migrate the database
            MigrateDatabase(app);
        }

        /// <summary>
        /// Migrates the database to the latest version if there are pending migrations.
        /// </summary>
        /// <param name="app">The application builder instance.</param>
        public static void MigrateDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<PharmacyDbContext>();

                // Check if there are any pending migrations and apply them
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
