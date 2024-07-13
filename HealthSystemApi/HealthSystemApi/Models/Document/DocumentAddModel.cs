using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Models.Document
{
    public class DocumentAddModel
    {
        public string? Type { get; set; }

        public string? Title { get; set; }

        public string? Notes { get; set; }

        [Required]
        public string? FileName { get; set; }

        public int HealthIssueId { get; set; }

        [Required]
        public string? UserId { get; set; }
    }
}
