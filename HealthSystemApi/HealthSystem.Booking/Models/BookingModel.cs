using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Booking.Models
{
    public class BookingModel
    {
        [Required]
        public string? Time { get; set; }

        [Required]
        public int Day { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public string? UserId { get; set; }
    }
}
