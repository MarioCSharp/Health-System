using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSystemApi.Data.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [Required]
        public int ServiceId { get; set; }
        [ForeignKey(nameof(ServiceId))]
        public Service? Service { get; set; }

        [Required]
        public int DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public Doctor? Doctor { get; set; }
    }
}
