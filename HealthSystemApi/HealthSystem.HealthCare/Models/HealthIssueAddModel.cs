using System.ComponentModel.DataAnnotations;

namespace HealthSystem.HealthCare.Models
{
    public class HealthIssueAddModel
    {
        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime IssueStartDate { get; set; }

        public DateTime IssueEndDate { get; set; }
    }
}
