namespace HealthProject.Models
{
    public class MedicationAddModel
    {
        public string? Name { get; set; }

        public int Dose { get; set; }

        public string? Type { get; set; }

        public string? Note { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<TimeSpan> Times { get; set; } = new List<TimeSpan>();

        public int SkipCount { get; set; }

        public List<DayOfWeek> Days { get; set; } = new List<DayOfWeek>();

        public int Take { get; set; }

        public int Rest { get; set; }

        public string? UserId { get; set; }
    }
}
