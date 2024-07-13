using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Models.HealthIssue
{
    public class HealthIssueAddModel
    {
        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime IssueStartDate { get; set; }

        [Required]
        public DateTime IssueEndDate { get; set; }
    }
}
