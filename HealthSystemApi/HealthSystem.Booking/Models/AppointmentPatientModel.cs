namespace HealthSystem.Booking.Models
{
    public class AppointmentPatientModel
    {
        public int Id { get; set; }
        public string? ServiceName { get; set; }
        public string? PatientName { get; set; }
        public string? Date { get; set; }
    }
}
