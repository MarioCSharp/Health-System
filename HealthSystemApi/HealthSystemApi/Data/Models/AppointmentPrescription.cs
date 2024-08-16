using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Data.Models
{
    public class AppointmentPrescription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public byte[] File { get; set; }

        [Required]
        public int AppointmentId { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public Booking? Appointment { get; set; }
    }
}
