namespace HealthSystem.Results.Models
{
    public class IssueRecipeModel
    {
        public byte[] File { get; set; }

        public string? PatientEGN { get; set; }

        public string? PatientName { get; set; }

        public string? DoctorName { get; set; }
    }
}
