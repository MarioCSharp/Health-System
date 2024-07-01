using HealthProject.Models;

namespace HealthProject.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<bool> AddDoctorAsync(AddDoctorModel doctorModel);
        Task<bool> DetailsAsync(AddDoctorModel doctorModel);
    }
}
