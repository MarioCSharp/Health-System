using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Models.Cart;

namespace HealthSystem.Pharmacy.Services.CartService
{
    public class CartService : ICartService
    {
        private PharmacyDbContext context;

        public CartService(PharmacyDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddToCartAsync(AddToCartModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<CartDisplayModel> GetUserCartAsync(int pharmacyId, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveFromCartAsync(int cartItemId)
        {
            throw new NotImplementedException();
        }
    }
}
