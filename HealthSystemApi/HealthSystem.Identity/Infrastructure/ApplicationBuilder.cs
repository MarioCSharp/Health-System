using HealthSystem.Identity.Data;
using HealthSystem.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Identity.Infrastructure
{
    /// <summary>
    /// Provides extension methods for initializing the application, including database migration and seeding of the Administrator role.
    /// </summary>
    public static class ApplicationBuilder
    {
        /// <summary>
        /// Initializes the application by migrating the database and seeding the Administrator role.
        /// </summary>
        /// <param name="app">The application builder interface.</param>
        public static async void Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(app);
            SeedAdministrator(services);
        }

        /// <summary>
        /// Applies pending migrations to the database if there are any.
        /// </summary>
        /// <param name="app">The application builder interface.</param>
        public static void MigrateDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<IdentityDbContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
        }

        /// <summary>
        /// Seeds the default roles including Administrator, Director, Doctor, and more into the application.
        /// </summary>
        /// <param name="services">The service provider for accessing application services like UserManager and RoleManager.</param>
        public static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync("Administrator"))
                    {
                        return;
                    }

                    var admin = new IdentityRole("Administrator");
                    var director = new IdentityRole("Director");
                    var doctor = new IdentityRole("Doctor");
                    var recepcionist = new IdentityRole("Recepcionist");
                    var pharmacyOwner = new IdentityRole("PharmacyOwner");
                    var pharmacist = new IdentityRole("Pharmacist");

                    await roleManager.CreateAsync(admin);
                    await roleManager.CreateAsync(director);
                    await roleManager.CreateAsync(doctor);
                    await roleManager.CreateAsync(recepcionist);
                    await roleManager.CreateAsync(pharmacyOwner);
                    await roleManager.CreateAsync(pharmacist);

                    const string adminEmail = "mario_petkov2007@abv.bg";
                    const string adminPassword = "Admin@123";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        EmailConfirmed = true,
                        FullName = "Mario Petkov",
                    };

                    await userManager.CreateAsync(user, adminPassword);
                    await userManager.AddToRoleAsync(user, admin.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
