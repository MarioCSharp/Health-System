using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Models.Appointment
{
    public class AppointmentCommentAddModel
    {
        [Required]
        public string? Comment { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public string? Token { get; set; }
    }
}
