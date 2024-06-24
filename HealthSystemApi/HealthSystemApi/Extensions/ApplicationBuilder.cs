﻿using HealthSystemApi.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace HealthSystemApi.Extensions
{
    public static class ApplicationBuilder
    {
        public static async void Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            SeedAdministrator(services);
        }

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
                    var doctor = new IdentityRole("Doctor");

                    await roleManager.CreateAsync(admin);
                    await roleManager.CreateAsync(doctor);

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
