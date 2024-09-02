using System.ComponentModel.DataAnnotations;

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

        public virtual ICollection<OrderMedication> OrderMedications { get; set; } = new HashSet<OrderMedication>();
    }
}
