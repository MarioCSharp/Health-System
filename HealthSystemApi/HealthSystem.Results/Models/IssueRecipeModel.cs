namespace HealthSystem.Results.Models
{
    public class IssueRecipeModel
    {
        public string? DoctorName { get; set; }

        public string? PatientEGN { get; set; }

        public string? PatientName { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
