using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSystemApi.Data.Models
{
    public class Medication
    {
        [Key]
        public int Id { get; set; }

        public int HealthIssueId { get; set; }
        [ForeignKey(nameof(HealthIssueId))]
        public HealthIssue? HealthIssue { get; set; }

        public string? Name { get; set; }

        [Required]
        public int Dose { get; set; }

        [Required]
        public string? Type { get; set; }

        public string? Note { get; set; }

        [Required]
        public int MedicationScheduleId { get; set; }
        [ForeignKey(nameof(MedicationScheduleId))]
        public MedicationSchedule? MedicationSchedule { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
