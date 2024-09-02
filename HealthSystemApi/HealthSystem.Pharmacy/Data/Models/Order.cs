using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Pharmacy.Data.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        public virtual ICollection<OrderMedication> OrderMedications { get; set; } = new HashSet<OrderMedication>();
    }
}
