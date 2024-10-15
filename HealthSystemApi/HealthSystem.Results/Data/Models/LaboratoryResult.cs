using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Results.Data.Models
{
    public class LaboratoryResult
    {
        [Key]
        public int Id { get; set; }

        public byte[]? File { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public string? UserLogingName { get; set; }

        [Required]
        public string? UserLogingPass { get; set; }
        [Required]
        public byte[]? QR { get; set; }

        [Required]
        public string? PatientName { get; set; }

        [Required]
        public string? DoctorUserId { get; set; }
    }
}
