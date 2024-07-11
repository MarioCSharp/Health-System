using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSystemApi.Data.Models
{
    public class Problem
    {
        [Key]
        public int Id { get; set; }

        public string? Notes { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int HealthIssueId { get; set; }
        [ForeignKey(nameof(HealthIssueId))]
        public HealthIssue? HealthIssue { get; set; }

        [Required]
        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public ICollection<Symptom> Symptoms { get; } = new List<Symptom>();
    }
}
