using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Problems.Models
{
    public class ProblemAddModel
    {
        public string? Notes { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int HealthIssueId { get; set; }
    }
}
