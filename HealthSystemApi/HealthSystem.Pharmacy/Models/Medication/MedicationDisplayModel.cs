namespace HealthSystem.Pharmacy.Models.Medication
{
    public class MedicationDisplayModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public float Price { get; set; }

        public byte[]? Image { get; set; }

        public int Quantity { get; set; }
    }
}
