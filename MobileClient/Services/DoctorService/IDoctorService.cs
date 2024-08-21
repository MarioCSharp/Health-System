using HealthProject.Models;

namespace HealthProject.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<DoctorDetailsModel> DetailsAsync(int id);
        Task<List<DoctorModel>> AllAsync(int id);
        Task<AddDoctorModel> GetDoctor(int id);
    }
}
