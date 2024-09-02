using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Models.Pharmacist;

namespace HealthSystem.Pharmacy.Services.PharmacistService
{
    public class PharmacistService : IPharmacistService
    {
        private PharmacyDbContext context;

        public PharmacistService(PharmacyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(PharmacistAddModel model, string userId, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PharmacistDisplayModel>> AllByPharmacyId(int pharmacyId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int pharmacistId, string token)
        {
            throw new NotImplementedException();
        }
    }
}
