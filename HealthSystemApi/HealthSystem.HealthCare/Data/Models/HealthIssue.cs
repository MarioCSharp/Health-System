using System.ComponentModel.DataAnnotations;

namespace HealthSystem.HealthCare.Data.Models
{
    public class HealthIssue
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public DateTime IssueStartDate { get; set; }

        [Required]
        public DateTime IssueEndDate { get; set; }

        [Required]
        public string? UserId { get; set; }
    }
}
