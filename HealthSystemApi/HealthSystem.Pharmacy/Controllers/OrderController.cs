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
        [Authorize(Roles = "PharmacyOwner,Pharmacist")]
        public async Task<IActionResult> AllOrdersInPharmacy([FromQuery] int pharmacyId)
        {
            var result = await orderService.OrdersInPharmacyAsync(pharmacyId);

            return Ok(result);
        }

        [HttpGet("ChangeStatus")]
        [Authorize(Roles = "PharmacyOwner,Pharmacist")]
        public async Task<IActionResult> ChangeStatus([FromQuery] int orderId, string newStatus)
        {
            var result = await orderService.ChangeStatus(orderId, newStatus);

            return Ok(result);
        }

        [HttpGet("GetOrderByEGN")]
        [Authorize]
        public async Task<IActionResult> GetOrderByEGN([FromQuery] string EGN, int userCartId)
        {
            var result = await orderService.GetOrderByEGNAsync(EGN, userCartId);

            return result ? Ok() : BadRequest();
        }
    }
}
