namespace HealthProject.Models
{
    public class MakeBookingModel
    {
        public string? Time { get; set; }

        public int Day { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int DoctorId { get; set; }

        public int ServiceId { get; set; }

        public string? UserId { get; set; }
    }
}
