using HealthSystem.Identity.Data;
using HealthSystem.Identity.Data.Models;
using HealthSystem.Identity.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Identity.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private IdentityDbContext context;
        private UserManager<User> userManager;

        public AccountService(IdentityDbContext context,
                              UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

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
