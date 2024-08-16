using HealthSystemApi.Models.Appointment;

namespace HealthSystemApi.Services.AppointmentService
{
    public interface IAppointmentService
    {
        Task<List<AppointmentModel>> GetNextAppointmentsByDoctorUserId(string token);
        Task<List<AppointmentModel>> GetPastAppointmentsByDoctorUserId(string token);
        Task<bool> Remove(int id);
        Task<bool> AddComent(AppointmentCommentAddModel model);
        Task<(bool, IFormFile)> IssuePrescriptionAsync(PrescriptionModel model);
        Task<(bool, IFormFile)> HasPrescriptionAsync(int appointmentId);
    }
}
