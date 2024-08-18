using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthSystem.HealthCare.Data.Models
{
    public class MedicationSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public List<TimeSpan> Times { get; set; } = new List<TimeSpan>();

        [Required]
        public int SkipCount { get; set; }

        [Required]
        public List<DayOfWeek> Days { get; set; } = new List<DayOfWeek>();

        [Required]
        public int Take { get; set; }

        [Required]
        public int Rest { get; set; }

        [Required]
        public string? UserId { get; set; }
    }
}
