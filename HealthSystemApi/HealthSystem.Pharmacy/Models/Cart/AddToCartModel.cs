using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Pharmacy.Models.Cart
{
    public class AddToCartModel
    {
        [Required]
        public int MedicationId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int UserCartId { get; set; }
    }
}
