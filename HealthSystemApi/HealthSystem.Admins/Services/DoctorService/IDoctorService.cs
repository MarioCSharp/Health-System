using HealthSystem.Admins.Models;

namespace HealthSystem.Admins.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<bool> AddAsync(DoctorAddModel model, string userId);
        Task<bool> RemoveAsync(int id, string userId);
        Task<List<DoctorModel>> GetAllAsync(int id);
        Task<DoctorDetailsModel> GetDetailsAsync(int id);
        Task<DoctorAddModel> GetDoctor(int id);
        Task Edit(DoctorDetailsModel model, string userId);
        Task<DoctorModel> GetDoctorByUserId(string userId);
        Task<List<DoctorModel>> GetAllDoctorsByUserId(string userId);
        Task<int> HospitalIdByDirector(string userId);
    }
}
