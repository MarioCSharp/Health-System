using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Account;
using HealthSystemApi.Models.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApi.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private ApplicationDbContext context;
        private UserManager<User> userManager;

        public AccountService(ApplicationDbContext context,
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

        public async Task<(string, List<BookingDisplayModel>)> GetAppointments(string userId)
        {
            var apps = await context.Bookings
                .Where(x => x.UserId == userId)
                .Select(x => new BookingDisplayModel
                {
                    Id = x.Id,
                    Date = x.Date.ToString("dd/MM/yyyy HH:mm"),
                    Name = x.Doctor.User.FullName,
                    ServiceName = x.Service.Name
                }).ToListAsync();

            var user = await context.Users.FindAsync(userId);

            return (user.FullName, apps);
        }
    }
}
