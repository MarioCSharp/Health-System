using HealthSystemApi.Models.Doctor;
using HealthSystemApi.Models.Hospital;

namespace HealthSystemApi.Services.HospitalService
{
    public interface IHospitalService
    {
        Task<bool> AddAsync(HospitalAddModel model);
        Task<bool> RemoveAsync(int id);
        Task<List<HospitalModel>> AllAsync();
        Task<HospitalDetailsModel> HospitalDetails(int id);
        Task<List<DoctorDisplayModel>> GetDoctorsAsync(int id);
        Task<HospitalDetailsModel> GetHospital(int id);
        Task<bool> EditAsync(HospitalEditModel model);
        Task<HospitalDetailsModel> GetHospitalByToken(string token);
    }
}
