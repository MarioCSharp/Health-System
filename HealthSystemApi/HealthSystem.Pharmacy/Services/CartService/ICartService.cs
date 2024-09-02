using HealthSystem.Pharmacy.Models.Cart;

namespace HealthSystem.Pharmacy.Services.CartService
{
    public interface ICartService
    {
        Task<bool> AddToCartAsync(AddToCartModel model);
        Task<bool> RemoveFromCartAsync(int cartItemId);
        Task<CartDisplayModel> GetUserCartAsync(int pharmacyId, string userId);
    }
}
