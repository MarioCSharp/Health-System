using HealthSystemApi.Models.Doctor;

namespace HealthSystemApi.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<bool> AddAsync(DoctorAddModel model);
        Task<bool> RemoveAsync(int id);
        Task<List<DoctorModel>> GetAllAsync(int id);
        Task<DoctorDetailsModel> GetDetailsAsync(int id);
    }
}
