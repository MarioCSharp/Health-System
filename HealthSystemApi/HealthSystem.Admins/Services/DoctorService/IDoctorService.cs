using HealthSystem.Admins.Models;

namespace HealthSystem.Admins.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<bool> AddAsync(DoctorAddModel model);
        Task<bool> RemoveAsync(int id);
        Task<List<DoctorModel>> GetAllAsync(int id);
        Task<DoctorDetailsModel> GetDetailsAsync(int id);
        Task<DoctorAddModel> GetDoctor(int id);
        Task Edit(DoctorDetailsModel model);
        Task<DoctorModel> GetDoctorByUserId(string userId);
    }
}
