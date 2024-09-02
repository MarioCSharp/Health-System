using HealthSystem.Pharmacy.Data;

namespace HealthSystem.Pharmacy.Services.PharmacyService
{
    public class PharmacyService : IPharmacyService
    {
        private PharmacyDbContext context;

        public PharmacyService(PharmacyDbContext context)
        {
            this.context = context;
        }
    }
}
