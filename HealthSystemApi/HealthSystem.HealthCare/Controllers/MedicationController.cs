using HealthSystem.HealthCare.Models;
using HealthSystem.HealthCare.Services.MedicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.HealthCare.Controllers
{
    /// <summary>
    /// API Controller for managing medications for users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MedicationController : ControllerBase
    {
        private readonly IMedicationService medicationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicationController"/> class.
        /// </summary>
        /// <param name="medicationService">Service for managing medications.</param>
        public MedicationController(IMedicationService medicationService)
        {
            this.medicationService = medicationService;
        }

        /// <summary>
        /// Adds a new medication entry.
        /// </summary>
        /// <param name="medicationModel">The medication model containing the details to add.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success or failure of the operation.</returns>
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

        /// <summary>
        /// Removes a medication entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the medication to remove.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the removal operation.</returns>
        [HttpGet("Remove")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            return Ok(await medicationService.DeleteAsync(id));
        }

        /// <summary>
        /// Retrieves the details of a medication entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the medication entry.</param>
        /// <returns>An <see cref="IActionResult"/> with the details of the medication.</returns>
        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            return Ok(await medicationService.DetailsAsync(id));
        }

        /// <summary>
        /// Retrieves all medications for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose medications are to be retrieved.</param>
        /// <returns>An <see cref="IActionResult"/> with the medications of the user.</returns>
        [HttpGet("UserMedicaiton")]
        public async Task<IActionResult> UserMedicaiton([FromQuery] string userId)
        {
            return Ok(await medicationService.AllByUser(userId));
        }

        /// <summary>
        /// Retrieves the medication schedule for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose medication schedule is to be retrieved.</param>
        /// <returns>An <see cref="IActionResult"/> with the user's medication schedule.</returns>
        [HttpGet("UserSchedule")]
        public async Task<IActionResult> GetSchedule([FromQuery] string userId)
        {
            return Ok(await medicationService.GetUserScheduleAsync(userId));
        }

        /// <summary>
        /// Retrieves the valid medications for the authenticated user.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> with the user's valid medications.</returns>
        [HttpGet("GetUsersValidMedications")]
        [Authorize]
        public async Task<IActionResult> GetUsersValidMedications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            return Ok(await medicationService.GetUsersValidMedications(userId));
        }
    }
}
