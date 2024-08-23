namespace HealthSystem.Admins.Models
{
    public class AppointmentModel
    {
        public string? UserId { get; set; }
        public int DoctorId { get; set; }
        public string? Date { get; set; }
    }
}
