using HealthSystem.Admins.Models;

namespace HealthSystem.Admins.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<bool> AddAsync(DoctorAddModel model, string userId, string token);
        Task<bool> RemoveAsync(int id, string userId, string token);
        Task<List<DoctorModel>> GetAllAsync(int id);
        Task<DoctorDetailsModel> GetDetailsAsync(int id);
        Task<DoctorAddModel> GetDoctor(int id);
        Task Edit(DoctorDetailsModel model, string userId);
        Task<DoctorModel> GetDoctorByUserId(string userId);
        Task<List<DoctorModel>> GetAllDoctorsByUserId(string userId);
        Task<int> HospitalIdByDirector(string userId);
        Task<bool> AddRating(float rating, string comment, int doctorId, int appointmentId, string userId);
        Task<bool> AppointmentHasRating(int appointmentId);
        Task<List<DoctorModel>> GetTopDoctorsWithSpecialization(string specialization, int top);
        Task<List<DoctorRatingDisplayModel>> GetDoctorRatings(int doctorId);
    }
}
