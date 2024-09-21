using HealthSystem.Pharmacy.Models.Cart;
using HealthSystem.Pharmacy.Services.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Pharmacy.Controllers
{
    /// <summary>
    /// API controller responsible for handling cart operations in the pharmacy system.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private ICartService cartService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="cartService">The <see cref="ICartService"/> responsible for handling cart-related operations.</param>
        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        /// <summary>
        /// Adds an item to the user's cart.
        /// </summary>
        /// <param name="model">The <see cref="AddToCartModel"/> containing details of the item to add.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost("AddToCart")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromForm] AddToCartModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await cartService.AddToCartAsync(model);

            return result ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Retrieves the current user's cart for the specified pharmacy.
        /// </summary>
        /// <param name="pharmacyId">The ID of the pharmacy for which to retrieve the cart.</param>
        /// <returns>An <see cref="IActionResult"/> containing the user's cart information.</returns>
        [HttpGet("GetUserCart")]
        [Authorize]
        public async Task<IActionResult> GetUserCart([FromQuery] int pharmacyId)
        {
            var result = await cartService.GetUserCartAsync(pharmacyId, User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(result);
        }

        /// <summary>
        /// Removes an item from the user's cart.
        /// </summary>
        /// <param name="cartItemId">The ID of the cart item to remove.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpGet("RemoveFromCart")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart([FromQuery] int cartItemId)
        {
            var result = await cartService.RemoveFromCartAsync(cartItemId);

            return Ok(result);
        }
    }
}
