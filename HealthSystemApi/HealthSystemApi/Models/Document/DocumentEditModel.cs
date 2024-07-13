namespace HealthSystemApi.Models.Document
{
    public class DocumentEditModel
    {
        public int Id { get; set; }
        public string? Type { get; set; }

        public string? Title { get; set; }

        public string? Notes { get; set; }

        public int HealthIssueId { get; set; }
    }
}
