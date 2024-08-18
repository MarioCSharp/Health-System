namespace HealthSystem.HealthCare.Models
{
    public class MedicationDisplayModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Dose { get; set; }

        public string? Type { get; set; }

        public int MedicationScheduleId { get; set; }
    }
}
