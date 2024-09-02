using HealthSystem.Pharmacy.Models.Pharmacist;

namespace HealthSystem.Pharmacy.Services.PharmacistService
{
    public interface IPharmacistService
    {
        Task<bool> AddAsync(PharmacistAddModel model, string userId, string token);
        Task<List<PharmacistDisplayModel>> AllByPharmacyId(int pharmacyId);
        Task<bool> DeleteAsync(int pharmacistId, string token);
    }
}
