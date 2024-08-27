using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Admins.Models
{
    public class DoctorAddModel
    {
        [Required]
        public string? Specialization { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public string? About { get; set; }
        [Required]
        public string? ContactNumber { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? FullName { get; set; }
        [Required]
        public int HospitalId { get; set; }
    }
}
