using HealthProject.Models;

namespace HealthProject.Services.HospitalService
{
    public interface IHospitalService
    {
        Task Add(AddHospitalModel model);
    }
}
