using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Pharmacy.Data.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserCartId { get; set; }

        [ForeignKey(nameof(UserCartId))]
        public UserCart? UserCart { get; set; }

        [Required]
        public int MedicationId { get; set; }

        [ForeignKey(nameof(MedicationId))]
        public Medication? Medication { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
