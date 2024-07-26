using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Models.Medication
{
    public class MedicationAddModel
    {
        public int HealthIssueId { get; set; }

        public string? Name { get; set; }

        [Required]
        public int Dose { get; set; }

        [Required]
        public string? Type { get; set; }

        public string? Note { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
