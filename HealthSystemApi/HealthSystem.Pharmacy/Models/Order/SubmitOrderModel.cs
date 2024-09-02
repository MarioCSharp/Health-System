using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Pharmacy.Models.Order
{
    public class SubmitOrderModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public int CartId { get; set; }
    }
}
