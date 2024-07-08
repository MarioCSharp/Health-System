using HealthSystemApi.Models.Service;

namespace HealthSystemApi.Services.ServiceService
{
    public interface IServiceService
    {
        Task<bool> AddAsync(ServiceAddModel model);
        Task<List<ServiceModel>> AllByIdAsync(int id);
        Task<ServiceDetailsModel> DetailsAsync(int id);
        Task<bool> BookAsync(BookingModel model);
    }
}
