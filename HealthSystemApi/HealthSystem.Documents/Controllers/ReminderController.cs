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

        public ReminderController(IReminderService reminderService)
        {
            this.reminderService = reminderService;
        }

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

        [HttpGet("Remove")]
        [Authorize]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            var result = await reminderService.RemoveAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "invalid");

            return result ? Ok(result) : BadRequest();
        }

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
