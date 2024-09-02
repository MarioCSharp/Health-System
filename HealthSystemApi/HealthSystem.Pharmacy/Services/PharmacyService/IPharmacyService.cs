using HealthSystem.Pharmacy.Models.Pharmacy;

namespace HealthSystem.Pharmacy.Services.PharmacyService
{
    public interface IPharmacyService
    {
        Task<bool> AddAsync(PharmacyAddModel model, string token);
        Task<List<PharmacyModel>> AllAsync();
        Task<PharmacyModel> DetailsAsync();
        Task<bool> DeleteAsync(int id);
        Task<bool> EditAsync(PharmacyEditModel model, string userId);
    }
}
