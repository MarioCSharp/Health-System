using System.ComponentModel.DataAnnotations;

namespace HealthProject.Models
{
    public class ProblemAddModel
    {
        public string? Notes { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int HealthIssueId { get; set; }

        public List<int> SelectedSymptoms { get; set; } = new List<int>();
    }
}
