using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Documents.Data.Models
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public DateTime RemindTime { get; set; }

        [Required]
        public string? UserId { get; set; }
    }
}
