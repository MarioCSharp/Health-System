using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Models.Problem
{
    public class ProblemAddModel
    {
        public string? Notes { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int HealthIssueId { get; set; }
    }
}
