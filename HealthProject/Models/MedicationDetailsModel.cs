namespace HealthProject.Models
{
    public class MedicationDetailsModel
    {
        public int HealthIssueId { get; set; }

        public string? Name { get; set; }

        public int Dose { get; set; }

        public string? Type { get; set; }

        public string? Note { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
