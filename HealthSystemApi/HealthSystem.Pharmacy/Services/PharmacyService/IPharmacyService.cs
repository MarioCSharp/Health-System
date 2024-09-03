using HealthSystem.Pharmacy.Models.Pharmacy;

namespace HealthSystem.Pharmacy.Services.PharmacyService
{
    public interface IPharmacyService
    {
        Task<bool> AddAsync(PharmacyAddModel model, string token);
        Task<List<PharmacyModel>> AllAsync();
        Task<PharmacyModel> DetailsAsync(int id);
        Task<bool> DeleteAsync(int id, string token);
        Task<bool> EditAsync(PharmacyEditModel model, string userId);
        Task<PharmacyModel> GetPharmacyByUserIdAsync(string userId, string role);
    }
}
