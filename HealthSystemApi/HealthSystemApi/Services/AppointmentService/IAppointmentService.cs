using HealthSystemApi.Models.Appointment;

namespace HealthSystemApi.Services.AppointmentService
{
    public interface IAppointmentService
    {
        Task<List<AppointmentModel>> GetNextAppointmentsByDoctorUserId(string token);
        Task<List<AppointmentModel>> GetPastAppointmentsByDoctorUserId(string token);
        Task<bool> Remove(int id);
    }
}
