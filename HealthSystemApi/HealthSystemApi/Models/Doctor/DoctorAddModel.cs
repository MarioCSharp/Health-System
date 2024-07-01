namespace HealthSystemApi.Models.Doctor
{
    public class DoctorAddModel
    {
        public string? Specialization { get; set; }
        public string? UserId { get; set; }
        public int HospitalId { get; set; }
    }
}
