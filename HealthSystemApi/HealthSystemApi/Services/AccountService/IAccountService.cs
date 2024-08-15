using HealthSystemApi.Models.Service;

namespace HealthSystemApi.Services.AccountService
{
    public interface IAccountService
    {
        Task<(string, List<BookingDisplayModel>)> GetAppointments(string userId);
    }
}
