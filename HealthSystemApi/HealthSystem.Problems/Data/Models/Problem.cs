using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Problems.Data.Models
{
    public class Problem
    {
        [Key]
        public int Id { get; set; }

        public string? Notes { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int HealthIssueId { get; set; }

        [Required]
        public string? UserId { get; set; }

        public ICollection<Symptom> Symptoms { get; } = new List<Symptom>();
    }
}
