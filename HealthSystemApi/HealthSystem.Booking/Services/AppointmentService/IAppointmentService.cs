using HealthSystem.Booking.Models;

namespace HealthSystem.Booking.Services.AppointmentService
{
    public interface IAppointmentService
    {
        Task<List<AppointmentPatientModel>> GetNextAppointmentsByDoctorUserId(string token);
        Task<List<AppointmentPatientModel>> GetPastAppointmentsByDoctorUserId(string token);
        Task<bool> Remove(int id);
        Task<bool> AddComent(AppointmentCommentAddModel model);
        Task<(bool, IFormFile)> IssuePrescriptionAsync(PrescriptionModel model);
        Task<(bool, IFormFile)> HasPrescriptionAsync(int appointmentId);
        Task<(string, List<BookingDisplayModel>)> GetDoctorAppointments(int doctorId);
        Task<bool> RemoveAppointment(int appointmetId);
        Task DeleteAllByDoctorId(int doctor);
        Task<(string, List<BookingDisplayModel>)> GetUserAppointments(string userId);
    }
}
