using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Models.Pharmacy;

namespace HealthSystem.Pharmacy.Services.PharmacyService
{
    public class PharmacyService : IPharmacyService
    {
        private PharmacyDbContext context;

        public PharmacyService(PharmacyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(PharmacyAddModel model, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PharmacyModel>> AllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PharmacyModel> DetailsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EditAsync(PharmacyEditModel model, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
