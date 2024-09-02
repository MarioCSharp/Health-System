using HealthSystem.Pharmacy.Models.Cart;
using HealthSystem.Pharmacy.Services.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Pharmacy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private ICartService cartService;

        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

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

        [HttpGet("GetUserCart")]
        [Authorize]
        public async Task<IActionResult> GetUserCart([FromQuery] int pharmacyId)
        {
            var result = await cartService.GetUserCartAsync(pharmacyId, User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(result);
        }

        [HttpGet("RemoveFromCart")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart([FromQuery] int cartItemId)
        {
            var result = await cartService.RemoveFromCartAsync(cartItemId);

            return Ok(result);
        }
    }
}
