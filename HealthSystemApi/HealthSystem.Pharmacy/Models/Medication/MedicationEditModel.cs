namespace HealthSystem.Pharmacy.Models.Medication
{
    public class MedicationEditModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public float Price { get; set; }

        public IFormFile Image { get; set; }
    }
}
