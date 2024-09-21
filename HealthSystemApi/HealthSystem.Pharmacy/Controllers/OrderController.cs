using HealthSystem.Pharmacy.Models.Order;
using HealthSystem.Pharmacy.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Pharmacy.Controllers
{
    /// <summary>
    /// API controller responsible for managing order operations in the pharmacy system.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private IOrderService orderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderService">The <see cref="IOrderService"/> responsible for order-related operations.</param>
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        /// <summary>
        /// Submits a new order for the current user.
        /// </summary>
        /// <param name="model">The <see cref="SubmitOrderModel"/> containing the order details.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the order submission.</returns>
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

        /// <summary>
        /// Retrieves all orders in a specified pharmacy.
        /// </summary>
        /// <param name="pharmacyId">The ID of the pharmacy to retrieve orders for.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of orders.</returns>
        [HttpGet("AllOrdersInPharmacy")]
        [Authorize(Roles = "PharmacyOwner,Pharmacist")]
        public async Task<IActionResult> AllOrdersInPharmacy([FromQuery] int pharmacyId)
        {
            var result = await orderService.OrdersInPharmacyAsync(pharmacyId);

            return Ok(result);
        }

        /// <summary>
        /// Changes the status of a specified order.
        /// </summary>
        /// <param name="orderId">The ID of the order to change the status of.</param>
        /// <param name="newStatus">The new status to set for the order.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the status change.</returns>
        [HttpGet("ChangeStatus")]
        [Authorize(Roles = "PharmacyOwner,Pharmacist")]
        public async Task<IActionResult> ChangeStatus([FromQuery] int orderId, string newStatus)
        {
            var result = await orderService.ChangeStatus(orderId, newStatus);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves an order by EGN (Personal Identification Number) and user cart ID.
        /// </summary>
        /// <param name="EGN">The EGN of the user to retrieve the order for.</param>
        /// <param name="userCartId">The ID of the user's cart.</param>
        /// <returns>An <see cref="IActionResult"/> containing the order details.</returns>
        [HttpGet("GetOrderByEGN")]
        [Authorize]
        public async Task<IActionResult> GetOrderByEGN([FromQuery] string EGN, int userCartId)
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var result = await orderService.GetOrderByEGNAsync(EGN, userCartId, token);

            return result ? Ok(result) : BadRequest();
        }
    }
}
