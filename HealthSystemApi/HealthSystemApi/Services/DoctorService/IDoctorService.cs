using HealthSystemApi.Models.Doctor;
using HealthSystemApi.Models.Service;

namespace HealthSystemApi.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<bool> AddAsync(DoctorAddModel model);
        Task<bool> RemoveAsync(int id);
        Task<List<DoctorModel>> GetAllAsync(int id);
        Task<DoctorDetailsModel> GetDetailsAsync(int id);
        Task<DoctorAddModel> GetDoctor(int id);
        Task Edit(DoctorDetailsModel model);
        Task<(string, List<BookingDisplayModel>)> GetDoctorAppointments(int doctorId);
        Task<bool> RemoveAppointment(int appointmetId);
    }
}
