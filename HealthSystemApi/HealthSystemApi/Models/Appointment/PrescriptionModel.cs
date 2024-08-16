using System.ComponentModel.DataAnnotations;

namespace HealthSystemApi.Models.Appointment
{
    public class PrescriptionModel
    {
        [Required]
        public string? FullName { get; set; }

        [Required]
        public string? DateOfBirth { get; set; }

        [Required]
        public string? EGN { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? Complaints { get; set; }

        [Required]
        public string? Diagnosis { get; set; }

        [Required]
        public string? Conditions { get; set; }

        [Required]
        public string? Status { get; set; }

        [Required]
        public string? Therapy { get; set; }

        [Required]
        public string? Tests { get; set; }

        [Required]
        public string? DoctorName { get; set; }

        [Required]
        public string? UIN { get; set; }

        public string? Token { get; set; }
        public int AppointmentId { get; set; }
    }
}
