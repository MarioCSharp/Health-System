using HealthProject.Models;

namespace HealthProject.Services.ServiceService
{
    public interface IServiceService
    {
        Task AddAsync(ServiceAddModel model);
        Task<List<ServiceModel>> AllByIdAsync(int id);
    }
}
