using System.ComponentModel.DataAnnotations;

namespace HealthProject.Models
{
    public class DocumentDetailsModel
    {
        public int Id { get; set; }

        public string? Type { get; set; }

        public string? Title { get; set; }

        public string? Notes { get; set; }

        [Required]
        public byte[] FileName { get; set; }

        public string? HealthIssueName { get; set; }
    }
}
