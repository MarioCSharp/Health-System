using HealthSystem.HealthCare.Models;
using HealthSystem.HealthCare.Services.LogbookService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.HealthCare.Controllers
{
    /// <summary>
    /// API Controller for managing user logbooks.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LogbookController : ControllerBase
    {
        private readonly ILogbookService logbookService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogbookController"/> class.
        /// </summary>
        /// <param name="logbookService">Service for managing logbook entries.</param>
        public LogbookController(ILogbookService logbookService)
        {
            this.logbookService = logbookService;
        }

        /// <summary>
        /// Adds a new logbook entry.
        /// </summary>
        /// <param name="model">The log entry model to add.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success or failure of the operation.</returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] LogAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = await logbookService.AddAsync(model);

            if (!result)
            {
                return Ok(false);
            }

            return Ok(result);
        }

        /// <summary>
        /// Retrieves the logbook entry for editing based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the logbook entry to edit.</param>
        /// <returns>An <see cref="IActionResult"/> with the logbook entry data.</returns>
        [HttpGet("Edit")]
        public async Task<IActionResult> Edit([FromQuery] int id)
        {
            return Ok(await logbookService.GetEditAsync(id));
        }

        /// <summary>
        /// Edits an existing logbook entry.
        /// </summary>
        /// <param name="model">The log entry model with updated information.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success or failure of the operation.</returns>
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromForm] LogAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await logbookService.EditAsync(model);

            if (!result)
            {
                return Ok(false);
            }

            return Ok(result);
        }

        /// <summary>
        /// Deletes a logbook entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the logbook entry to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the deletion.</returns>
        [HttpGet("Remove")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await logbookService.DeleteAsync(id));
        }

        /// <summary>
        /// Retrieves all logbook entries for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose log entries are to be retrieved.</param>
        /// <returns>An <see cref="IActionResult"/> with the user's logbook entries.</returns>
        [HttpGet("AllByUser")]
        public async Task<IActionResult> AllByUser([FromQuery] string userId)
        {
            return Ok(await logbookService.AllByUserAsync(userId));
        }
    }
}
