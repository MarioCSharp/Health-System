namespace HealthSystem.Pharmacy.Models.Medication
{
    public class MedicationAddModel
    {
        public string? MedicationName { get; set; }
        public int MedicationQuantity { get; set; }
        public float MedicationPrice { get; set; }
        public IFormFile Image { get; set; }
        public int PharmacyId { get; set; }
    }
}
