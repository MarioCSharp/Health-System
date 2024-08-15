namespace HealthSystemApi.Models.Doctor
{
    public class DoctorDisplayModel
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Specialization { get; set; }
    }
}
