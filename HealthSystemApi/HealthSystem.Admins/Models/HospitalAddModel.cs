using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Admins.Models
{
    public class HospitalAddModel
    {
        [Required]
        public string? HospitalName { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        public string? ContactNumber { get; set; }

        [Required]
        public string? OwnerId { get; set; }
    }
}
