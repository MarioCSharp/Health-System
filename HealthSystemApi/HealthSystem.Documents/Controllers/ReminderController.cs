using HealthSystem.Documents.Models;
using HealthSystem.Documents.Services.ReminderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Documents.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReminderController : ControllerBase
    {
        private IReminderService reminderService;

        /// <summary>
        /// Initializes a new instance of the ReminderController class.
        /// </summary>
        /// <param name="reminderService">The reminder service used for managing reminders.</param>
        public ReminderController(IReminderService reminderService)
        {
            this.reminderService = reminderService;
        }

        /// <summary>
        /// Adds a new reminder asynchronously.
        /// </summary>
        /// <param name="model">The reminder model containing the details to add.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> Add([FromForm] ReminderAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var result = await reminderService.AddAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "invalid");

            return result ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Removes a reminder asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the reminder to remove.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpGet("Remove")]
        [Authorize]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            var result = await reminderService.RemoveAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "invalid");

            return result ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Retrieves all reminders for the current user asynchronously.
        /// </summary>
        /// <returns>An IActionResult containing a list of reminders for the user.</returns>
        [HttpGet("AllByUser")]
        [Authorize]
        public async Task<IActionResult> AllByUser()
        {
            var result = await reminderService.AllByUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "invalid");

            if (result is null)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
