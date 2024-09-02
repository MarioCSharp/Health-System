using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Models.Medication;

namespace HealthSystem.Pharmacy.Services.MedicationService
{
    public class MedicationService : IMedicationService
    {
        private PharmacyDbContext context;

        public MedicationService(PharmacyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(MedicationAddModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddQuantityAsync(int medicationId, int quantity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MedicationDisplayModel>> AllInPharmacyAsync(int pharmacyId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EditAsync(MedicationEditModel model)
        {
            throw new NotImplementedException();
        }
    }
}
