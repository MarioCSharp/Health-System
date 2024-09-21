using HealthSystem.Pharmacy.Data;
using HealthSystem.Pharmacy.Data.Models;
using HealthSystem.Pharmacy.Models.Cart;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Pharmacy.Services.CartService
{
    /// <summary>
    /// Service responsible for handling operations related to the shopping cart in the pharmacy system.
    /// </summary>
    public class CartService : ICartService
    {
        private PharmacyDbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartService"/> class.
        /// </summary>
        /// <param name="context">The <see cref="PharmacyDbContext"/> used for database operations.</param>
        public CartService(PharmacyDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds an item to the user's cart or updates the quantity if the item already exists in the cart.
        /// </summary>
        /// <param name="model">The <see cref="AddToCartModel"/> containing the details of the item to add or update.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
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

        /// <summary>
        /// Retrieves the shopping cart for a specific user in a given pharmacy.
        /// </summary>
        /// <param name="pharmacyId">The ID of the pharmacy.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A <see cref="CartDisplayModel"/> containing the cart details, including items and total price.</returns>
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

        /// <summary>
        /// Removes an item from the user's cart.
        /// </summary>
        /// <param name="cartItemId">The ID of the cart item to remove.</param>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
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
