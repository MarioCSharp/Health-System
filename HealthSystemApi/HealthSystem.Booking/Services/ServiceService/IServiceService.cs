using HealthSystem.Booking.Models;

namespace HealthSystem.Booking.Services.ServiceService
{
    public interface IServiceService
    {
        Task<bool> AddAsync(ServiceAddModel model);
        Task<(string, List<ServiceModel>)> AllByIdAsync(int id);
        Task<List<ServiceModel>> AllByUserIdAsync(string userId);
        Task<ServiceDetailsModel> DetailsAsync(int id);
        Task<bool> BookAsync(BookingModel model);
        Task<List<string>> AvailableHoursAsync(DateTime date, int serviceId);
        Task<List<AppointmentModel>> AllByUserAsync(string userId);
        Task<bool> Delete(int id);
        Task<(string, decimal, string, string)> EditGET(int id);
        Task<bool> EditPOST(ServiceEditModel model);
        Task DeleteAllByDoctorId(int doctorId);
    }
}
