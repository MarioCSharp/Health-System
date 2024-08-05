namespace HealthProject.Models
{
    public class LogAddModel
    {
        public int Id { get; set; }

        public string? Type { get; set; }

        public int HealthIssueId { get; set; }

        public List<int> Values { get; set; } = new List<int>();

        public List<string> Factors { get; set; } = new List<string>();

        public string? Note { get; set; }

        public string? UserId { get; set; }

        public DateTime Date { get; set; }
    }
}
