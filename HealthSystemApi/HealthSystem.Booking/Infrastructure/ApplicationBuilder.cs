using HealthSystem.Booking.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Booking.Infrastructure
{
    public static class ApplicationBuilder
    {
        /// <summary>
        /// Initializes the application by setting up the necessary infrastructure.
        /// </summary>
        /// <param name="app">The application builder used to configure the app.</param>
        public static async void Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(app);
        }

        /// <summary>
        /// Applies any pending database migrations.
        /// </summary>
        /// <param name="app">The application builder to access services and perform migrations.</param>
        public static void MigrateDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<BookingDbContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
