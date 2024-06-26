using HealthSystemApi.Models.Hospital;

namespace HealthSystemApi.Services.HospitalService
{
    public interface IHospitalService
    {
        Task<bool> AddAsync(HospitalAddModel model);
        Task<bool> RemoveAsync(int id);
        Task<List<HospitalModel>> AllAsync();
    }
}
