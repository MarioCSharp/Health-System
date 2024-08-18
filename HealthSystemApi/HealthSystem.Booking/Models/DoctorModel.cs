namespace HealthSystem.Booking.Models
{
    public class DoctorModel
    {
        public int Id { get; set; }
        public string? Specialization { get; set; }
        public string? UserId { get; set; }
        public string? About { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public int HospitalId { get; set; }
    }
}
