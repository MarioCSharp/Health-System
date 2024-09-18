using HealthSystem.Admins.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Admins.Infrastructure
{
    public static class ApplicationBuilder
    {
        /// <summary>
        /// Initializes the application by applying any pending migrations to the database.
        /// </summary>
        /// <param name="app">The application builder instance.</param>
        public static async void Initialize(this IApplicationBuilder app)
        {
            // Create a scope to resolve dependencies.
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            // Migrate the database if there are any pending migrations.
            MigrateDatabase(app);
        }

        /// <summary>
        /// Applies any pending migrations to the database.
        /// </summary>
        /// <param name="app">The application builder instance.</param>
        public static void MigrateDatabase(IApplicationBuilder app)
        {
            // Create a scope to resolve dependencies for database migration.
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                // Get the database context from the service provider.
                var context = services.GetRequiredService<AdminsDbContext>();

                // Check if there are any pending migrations and apply them.
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
