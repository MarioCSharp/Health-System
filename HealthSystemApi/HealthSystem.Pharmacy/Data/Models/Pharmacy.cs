using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Pharmacy.Data.Models
{
    public class Pharmacy
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
        public string? OwnerUserId { get; set; }
    }
}
