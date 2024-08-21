using HealthSystem.Admins.Models;

namespace HealthSystem.Admins.Services.HospitalService
{
    public interface IHospitalService
    {
        Task<bool> AddAsync(HospitalAddModel model);
        Task<bool> RemoveAsync(int id);
        Task<List<HospitalModel>> AllAsync();
        Task<HospitalDetailsModel> HospitalDetails(int id);
        Task<List<DoctorDisplayModel>> GetDoctorsAsync(int id, string userId);
        Task<HospitalDetailsModel> GetHospital(int id, string userId);
        Task<bool> EditAsync(HospitalEditModel model, string userId);
        Task<HospitalDetailsModel> GetHospitalByToken(string token);
    }
}
