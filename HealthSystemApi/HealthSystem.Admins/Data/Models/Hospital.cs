using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Admins.Data.Models
{
    public class Hospital
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public string? ContactNumber { get; set; }

        [Required]
        public string? OwnerId { get; set; }
    }
}
