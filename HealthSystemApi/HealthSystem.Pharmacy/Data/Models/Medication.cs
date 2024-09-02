using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSystem.Pharmacy.Data.Models
{
    public class Medication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? MedicationName { get; set; }

        [Required]
        public int MedicationQuantity { get; set; }

        [Required]
        public float MedicationPrice { get; set; }

        [Required]
        public int PharmacyId { get; set; }
        [ForeignKey(nameof(PharmacyId))]
        public Pharmacy? Pharmacy { get; set; }
    }
}
