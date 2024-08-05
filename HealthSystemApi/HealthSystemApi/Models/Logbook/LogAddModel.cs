using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Models.Logbook
{
    public class LogAddModel
    {
        public int Id { get; set; }

        [Required]
        public string? Type { get; set; }

        public int HealthIssueId { get; set; }

        [Required]
        public List<int> Values { get; set; } = new List<int>();

        [Required]
        public List<string> Factors { get; set; } = new List<string>();

        public string? Note { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
