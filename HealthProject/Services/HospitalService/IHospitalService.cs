using HealthProject.Models;

namespace HealthProject.Services.HospitalService
{
    public interface IHospitalService
    {
        Task<List<HospitalModel>> All();
        Task<HospitalDetailsModel> Details(int id);
    }
}
