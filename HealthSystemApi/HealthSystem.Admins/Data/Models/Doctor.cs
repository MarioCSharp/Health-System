using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthSystem.Admins.Data.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Specialization { get; set; }

        public string? FullName { get; set; }
        public string? Email { get; set; }


        [Required]
        public string? UserId { get; set; }

        [Required]
        public int HospitalId { get; set; }
        [ForeignKey(nameof(HospitalId))]
        public Hospital? Hospital { get; set; }
    }
}
