using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSystem.Admins.Data.Models
{
    public class Recepcionist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HospitalId { get; set; }
        [ForeignKey(nameof(HospitalId))]
        public Hospital? Hospital { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? UserId { get; set; }
    }
}
