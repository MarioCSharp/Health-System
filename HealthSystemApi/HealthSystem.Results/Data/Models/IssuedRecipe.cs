using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Results.Data.Models
{
    public class IssuedRecipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? EGN { get; set; }

        [Required]
        public byte[] File { get; set; }

        [Required]
        public string? DoctorName { get; set; }
        [Required]
        public string? PatientName { get; set; }
    }
}
