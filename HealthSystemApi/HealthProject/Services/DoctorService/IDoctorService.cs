using HealthProject.Models;

namespace HealthProject.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<bool> AddDoctorAsync(AddDoctorModel doctorModel);
        Task<DoctorDetailsModel> DetailsAsync(int id);
        Task<List<DoctorModel>> AllAsync(int id);
    }
}
