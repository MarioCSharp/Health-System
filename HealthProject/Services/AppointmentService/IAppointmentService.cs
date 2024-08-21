using HealthProject.Models;

namespace HealthProject.Services.AppointmentService
{
    public interface IAppointmentService
    {
        Task<List<AppointmentModel>> GetUserAppointmentsAsync(string userId);
        Task<List<PrescriptionDisplayModel>> GetUserPrescriptions(string userId);
    }
}
