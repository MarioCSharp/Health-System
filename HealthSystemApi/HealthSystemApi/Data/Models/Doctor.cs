using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSystemApi.Data.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Specialization { get; set; }

        [Required]
        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [Required]
        public int HospitalId { get; set; }
        [ForeignKey(nameof(HospitalId))]
        public Hospital? Hospital { get; set; }
    }
}
