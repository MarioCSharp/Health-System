using HealthSystem.Documents.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Documents.Infrastructure
{
    /// <summary>
    /// Extension methods for initializing and migrating the database.
    /// </summary>
    public static class ApplicationBuilder
    {
        /// <summary>
        /// Initializes the application by applying database migrations.
        /// </summary>
        /// <param name="app">The application builder instance.</param>
        public static async void Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(app);
        }

        /// <summary>
        /// Applies pending migrations to the database if any exist.
        /// </summary>
        /// <param name="app">The application builder instance.</param>
        public static void MigrateDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<DocumentsDbContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
