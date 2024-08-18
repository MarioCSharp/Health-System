using HealthProject.Models;

namespace HealthProject.Services.ServiceService
{
    public interface IServiceService
    {
        Task<List<ServiceModel>> AllByIdAsync(int id);
        Task<ServiceDetailsModel> DetailsAsync(int id);
        Task<List<string>> AvailableHoursAsync(DateTime date, int serviceId);
        Task<bool> BookAsync(MakeBookingModel model);
    }
}
