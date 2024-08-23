using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSystem.Admins.Data.Models
{
    public class DoctorRating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public float Rating { get; set; }

        [Required]
        public string? Comment { get; set; }

        [Required]
        public int DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public Doctor? Doctor { get; set; }

        [Required]
        public int AppointmentId { get; set; }
    }
}
