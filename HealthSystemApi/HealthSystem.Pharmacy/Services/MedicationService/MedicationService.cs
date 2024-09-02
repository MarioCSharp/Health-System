using HealthSystem.Pharmacy.Data;

namespace HealthSystem.Pharmacy.Services.MedicationService
{
    public class MedicationService : IMedicationService
    {
        private PharmacyDbContext context;

        public MedicationService(PharmacyDbContext context)
        {
            this.context = context;
        }
    }
}
