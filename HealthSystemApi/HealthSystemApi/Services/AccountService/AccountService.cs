using HealthSystemApi.Data;
using HealthSystemApi.Models.Service;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApi.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private ApplicationDbContext context;

        public AccountService(ApplicationDbContext context)
        {
            this.context = context;
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
