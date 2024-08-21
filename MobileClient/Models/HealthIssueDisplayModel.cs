using System.ComponentModel.DataAnnotations;

namespace HealthProject.Models
{
    public class HealthIssueDisplayModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public DateTime IssueStartDate { get; set; }

        public DateTime IssueEndDate { get; set; }
    }
}
