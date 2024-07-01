using HealthProject.Models;

namespace HealthProject.Services.HospitalService
{
    public interface IHospitalService
    {
        Task Add(AddHospitalModel model);
        Task<List<HospitalModel>> All();
        Task Delete(int id);
        Task<HospitalDetailsModel> Details(int id);
    }
}
