using HealthSystem.Pharmacy.Models.Medication;

namespace HealthSystem.Pharmacy.Services.MedicationService
{
    public interface IMedicationService
    {
        Task<bool> AddAsync(MedicationAddModel model);
        Task<bool> AddQuantityAsync(int medicationId, int quantity);
        Task<List<MedicationDisplayModel>> AllInPharmacyAsync(int pharmacyId);
        Task<bool> EditAsync(MedicationEditModel model);
    }
}
