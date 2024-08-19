namespace HealthSystem.Documents.Models
{
    public class PrescriptionDisplayModel
    {
        public string? Date { get; set; }
        public string? DoctorName { get; set; }
        public byte[] File { get; set; }
    }
}
