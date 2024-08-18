using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Booking.Data.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

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
    }
}
