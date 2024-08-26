using HealthSystem.HealthCare.Models;
using HealthSystem.HealthCare.Services.MedicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.HealthCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicationController : ControllerBase
    {
        private IMedicationService medicationService;

        public MedicationController(IMedicationService medicationService)
        {
            this.medicationService = medicationService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] MedicationAddModel medicationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await medicationService.AddAsync(medicationModel);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(true);
        }

        [HttpGet("Remove")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            return Ok(await medicationService.DeleteAsync(id));
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            return Ok(await medicationService.DetailsAsync(id));
        }

        [HttpGet("UserMedicaiton")]
        public async Task<IActionResult> UserMedicaiton([FromQuery] string userId)
        {
            return Ok(await medicationService.AllByUser(userId));
        }

        [HttpGet("UserSchedule")]
        public async Task<IActionResult> GetSchedule([FromQuery] string userId)
        {
            return Ok(await medicationService.GetUserScheduleAsync(userId));
        }

        [HttpGet("GetUsersValidMedications")]
        [Authorize]
        public async Task<IActionResult> GetUsersValidMedications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            return Ok(await medicationService.GetUsersValidMedications(userId));
        }
    }
}
