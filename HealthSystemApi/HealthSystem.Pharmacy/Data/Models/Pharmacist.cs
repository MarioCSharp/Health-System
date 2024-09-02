using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSystem.Pharmacy.Data.Models
{
    public class Pharmacist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public int PharmacyId { get; set; }
        [ForeignKey(nameof(PharmacyId))]
        public Pharmacy? Pharmacy { get; set; }
    }
}
