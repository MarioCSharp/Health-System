using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSystem.Booking.Data.Models
{
    public class AppointmentComment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Comment { get; set; }

        [Required]
        public int AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public Booking? Appointment { get; set; }
    }
}
