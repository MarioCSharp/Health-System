using System.ComponentModel.DataAnnotations;

namespace HealthProject.Models
{
    public class DocumentAddModel
    {
        public string? Type { get; set; }

        public string? Title { get; set; }

        public string? Notes { get; set; }

        public string? FilePath { get; set; }

        public string? FileName { get; set; }

        public string? FileContent { get; set; }

        public string? FileExtension { get; set; }

        public string? UserId { get; set; }

        public int HealthIssueId { get; set; }
    }
}
