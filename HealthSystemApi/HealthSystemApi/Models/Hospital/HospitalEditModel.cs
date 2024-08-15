namespace HealthSystemApi.Models.Hospital
{
    public class HospitalEditModel
    {
        public int Id { get; set; }
        public string? HospitalName { get; set; }
        public string? HospitalLocation { get; set; }
        public string? HospitalContactNumber { get; set; }
        public string? HospitalUserId { get; set; }
        public string? Token { get; set; }
    }
}
