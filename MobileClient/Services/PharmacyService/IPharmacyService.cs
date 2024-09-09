using HealthProject.Models;

namespace HealthProject.Services.PharmacyService
{
    public interface IPharmacyService
    {
        Task<List<PharmacyDisplayModel>> GetAll();
        Task<List<PharmacyProductDisplayModel>> GetAllProducts(int pharmacyId);
        Task<CartModel> GetUserCart(int pharmacyId);
        Task<bool> AddToCart(int medcicationId, int userCartId, int quantity);
        Task<bool> RemoveFromCart(int cartItemId);
        Task<bool> SubmitOrder(SubmitOrderModel model);
        Task<bool> GetMedicationsByEGNAsync(string EGN, int cartId);
    }
}
