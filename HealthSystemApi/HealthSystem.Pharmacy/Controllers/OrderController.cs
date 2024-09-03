using HealthSystem.Pharmacy.Models.Order;
using HealthSystem.Pharmacy.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Pharmacy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost("SubmitOrder")]
        [Authorize]
        public async Task<IActionResult> SubmitOrder([FromForm] SubmitOrderModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
            {
                return BadRequest();
            }

            var result = await orderService.SubmitOrderAsync(model, userId);

            return result ? Ok(result) : BadRequest();
        }

        [HttpGet("AllOrdersInPharmacy")]
        [Authorize(Roles = "Pharmacist")]
        public async Task<IActionResult> AllOrdersInPharmacy([FromQuery] int pharmacyId)
        {
            var result = await orderService.OrdersInPharmacyAsync(pharmacyId);

            return Ok(result);
        }
    }
}
