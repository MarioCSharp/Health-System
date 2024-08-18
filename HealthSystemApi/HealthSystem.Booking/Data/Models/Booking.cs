using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSystem.Booking.Data.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string? PatientName { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public int ServiceId { get; set; }
        [ForeignKey(nameof(ServiceId))]
        public Service? Service { get; set; }

        public string? DoctorName { get; set; }

        [Required]
        public int DoctorId { get; set; }
    }
}
