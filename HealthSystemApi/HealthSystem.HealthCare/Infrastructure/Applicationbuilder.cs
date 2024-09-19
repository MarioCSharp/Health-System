using HealthSystem.HealthCare.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.HealthCare.Infrastructure
{
    /// <summary>
    /// Provides extension methods for initializing and migrating the database.
    /// </summary>
    public static class ApplicationBuilder
    {
        /// <summary>
        /// Initializes the application by applying database migrations.
        /// </summary>
        /// <param name="app">The application builder instance used to configure the application's request pipeline.</param>
        public static async void Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            // Migrate the database to ensure all migrations are applied
            MigrateDatabase(app);
        }

        /// <summary>
        /// Applies any pending migrations for the database.
        /// </summary>
        /// <param name="app">The application builder instance used to configure the application's request pipeline.</param>
        public static void MigrateDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                // Retrieve the database context from the service provider
                var context = services.GetRequiredService<HealthCareDbContext>();

                // Check for any pending migrations and apply them
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
