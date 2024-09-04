using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Data.Models;
using HealthSystem.Pharmacy.Models.Cart;
using Microsoft.EntityFrameworkCore;

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
            var cartItem = await context.CartItems
                .FirstOrDefaultAsync(x => x.MedicationId == model.MedicationId && x.UserCartId == model.UserCartId);

            if (cartItem is null)
            {
                var newCartItem = new CartItem()
                {
                    MedicationId = model.MedicationId,
                    Quantity = model.Quantity,
                    UserCartId = model.UserCartId,
                };

                await context.CartItems.AddAsync(newCartItem);
                await context.SaveChangesAsync();

                return await context.CartItems.ContainsAsync(newCartItem);
            }

            cartItem.Quantity += model.Quantity;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<CartDisplayModel> GetUserCartAsync(int pharmacyId, string userId)
        {
            var userCart = await context.UserCarts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Medication)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.PharmacyId == pharmacyId);

            if (userCart is null)
            {
                var cart = new UserCart()
                {
                    PharmacyId = pharmacyId,
                    UserId = userId
                };

                await context.UserCarts.AddAsync(cart);
                await context.SaveChangesAsync();

                return new CartDisplayModel()
                {
                    TotalPrice = 0.0F,
                    CartItems = new List<CartItemModel>()
                };
            }

            var items = userCart.CartItems.Select(x => new CartItemModel()
            {
                Id = x.Id,
                ItemImage = x.Medication.Image,
                ItemName = x.Medication.MedicationName,
                ItemPrice = x.Medication.MedicationPrice,
                Quantity = x.Quantity
            }).ToList();

            return new CartDisplayModel()
            {
                Id = userCart.Id,
                CartItems = items,
                TotalPrice = items.Sum(x => x.ItemPrice * x.Quantity)
            };
        }

        public async Task<bool> RemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await context.CartItems.FindAsync(cartItemId);

            if (cartItem is null)
            {
                return false;
            }

            context.CartItems.Remove(cartItem);
            await context.SaveChangesAsync();

            return !await context.CartItems.ContainsAsync(cartItem);
        }
    }
}
