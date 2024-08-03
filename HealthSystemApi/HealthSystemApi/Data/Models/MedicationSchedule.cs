using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSystemApi.Data.Models
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
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
