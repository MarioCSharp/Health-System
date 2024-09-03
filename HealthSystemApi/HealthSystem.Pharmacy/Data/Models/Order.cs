using HealthSystem.Pharmacy.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSystem.Pharmacy.Data.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public int PharmacyId { get; set; }
        [ForeignKey(nameof(PharmacyId))]
        public Pharmacy? Pharmacy { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Placed;

        public virtual ICollection<OrderMedication> OrderMedications { get; set; } = new HashSet<OrderMedication>();
    }
}
