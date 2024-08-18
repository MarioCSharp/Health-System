namespace HealthSystem.HealthCare.Models
{
    public class HealthIssueDetailsModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime IssueStartDate { get; set; }

        public DateTime IssueEndDate { get; set; }
    }
}
