using HealthSystem.Identity.Data;
using HealthSystem.Identity.Data.Models;
using HealthSystem.Identity.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Identity.Services.AccountService
{
    /// <summary>
    /// Service for managing user accounts.
    /// </summary>
    public class AccountService : IAccountService
    {
        private IdentityDbContext context;
        private UserManager<User> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="context">The <see cref="IdentityDbContext"/> to access user data.</param>
        /// <param name="userManager">The <see cref="UserManager{User}"/> used to manage user roles.</param>
        public AccountService(IdentityDbContext context,
                              UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        /// <summary>
        /// Retrieves a list of user accounts that do not have any roles assigned.
        /// </summary>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. 
        /// The task result contains a list of <see cref="AccountDisplayModel"/> representing the users without roles.</returns>
        public async Task<List<AccountDisplayModel>> GetAccountsAsync()
        {
            var users = await context.Users.ToListAsync();

            var result = new List<AccountDisplayModel>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                if (roles.Count == 0)
                {
                    result.Add(new AccountDisplayModel
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email
                    });
                }
            }

            return result;
        }
    }
}
