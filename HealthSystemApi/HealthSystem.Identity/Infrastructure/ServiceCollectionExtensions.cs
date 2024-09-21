using HealthSystem.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HealthSystem.Identity.Infrastructure
{
    /// <summary>
    /// Contains extension methods for IServiceCollection related to user storage.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds user storage configuration to the IServiceCollection.
        /// Configures Identity for the <see cref="User"/> class with customized password settings.
        /// </summary>
        /// <param name="services">The IServiceCollection to which the user storage services are added.</param>
        /// <returns>The same <see cref="IServiceCollection"/> with user storage configured.</returns>
        public static IServiceCollection AddUserStorage(
            this IServiceCollection services)
        {
            services
                .AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                    .AddEntityFrameworkStores<IdentityDbContext>();

            return services;
        }
    }
}
