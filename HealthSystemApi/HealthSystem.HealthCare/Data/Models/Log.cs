using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthSystem.HealthCare.Data.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Type { get; set; }

        public int HealthIssueId { get; set; }
        [ForeignKey(nameof(HealthIssueId))]
        public HealthIssue? HealthIssue { get; set; }

        [Required]
        public List<int> Values { get; set; } = new List<int>();

        [Required]
        public List<string> Factors { get; set; } = new List<string>();

        public string? Note { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
