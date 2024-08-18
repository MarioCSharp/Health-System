using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Booking.Models
{
    public class ServiceAddModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public int DoctorId { get; set; }

        public string? Token { get; set; }
    }
}
