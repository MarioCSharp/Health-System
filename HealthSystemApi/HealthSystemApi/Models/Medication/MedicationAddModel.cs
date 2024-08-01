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
        [Required]
        public List<TimeSpan> Times { get; set; } = new List<TimeSpan>();
        [Required]
        public int SkipCount { get; set; }
        [Required]
        public List<DayOfWeek> Days { get; set; } = new List<DayOfWeek>();
        [Required]
        public int Take { get; set; }
        [Required]
        public int Rest { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public int MedicationId { get; set; }
    }
}
