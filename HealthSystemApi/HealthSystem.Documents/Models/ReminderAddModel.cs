using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Documents.Models
{
    public class ReminderAddModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public DateTime RemindTime { get; set; }
    }
}
