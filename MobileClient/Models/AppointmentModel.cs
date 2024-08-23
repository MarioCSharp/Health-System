namespace HealthProject.Models
{
    public class AppointmentModel
    {
        public int Id { get; set; }
        public string? ServiceName { get; set; }
        public string? DoctorName { get; set; }
        public string? Date { get; set; }

        public bool IsPast { get; set; }
    }
}
