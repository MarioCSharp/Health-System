using HealthSystem.HealthCare.Models;
using HealthSystem.HealthCare.Services.HealthIssueService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.HealthCare.Controllers
{
    /// <summary>
    /// API Controller for managing health issues related to users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HealthIssueController : ControllerBase
    {
        private readonly IHealthIssueService healthIssueService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthIssueController"/> class.
        /// </summary>
        /// <param name="healthIssueService">Service for managing health issues.</param>
        public HealthIssueController(IHealthIssueService healthIssueService)
        {
            this.healthIssueService = healthIssueService;
        }

        /// <summary>
        /// Adds a new health issue.
        /// </summary>
        /// <param name="startDate">The start date of the health issue (format: yyyy-MM-dd).</param>
        /// <param name="endDate">The end date of the health issue (format: yyyy-MM-dd).</param>
        /// <param name="name">The name of the health issue.</param>
        /// <param name="description">A description of the health issue.</param>
        /// <param name="userId">The ID of the user to whom the health issue belongs.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success or failure of the operation.</returns>
        [HttpGet("Add")]
        public async Task<IActionResult> Add([FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string name, [FromQuery] string description, [FromQuery] string userId)
        {
            if (!DateTime.TryParseExact(startDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedStartDate))
            {
                return BadRequest("Invalid start date format. Please use yyyy-MM-dd.");
            }

            if (!DateTime.TryParseExact(endDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedEndDate))
            {
                return BadRequest("Invalid end date format. Please use yyyy-MM-dd.");
            }

            var model = new HealthIssueAddModel()
            {
                Name = name,
                Description = description,
                IssueStartDate = parsedStartDate,
                IssueEndDate = parsedEndDate
            };

            var result = await healthIssueService.AddAsync(model, userId);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        /// <summary>
        /// Removes a health issue by its ID.
        /// </summary>
        /// <param name="id">The ID of the health issue to remove.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success or failure of the operation.</returns>
        [HttpGet("Remove")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            var result = await healthIssueService.RemoveAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        /// <summary>
        /// Retrieves the details of a health issue by its ID.
        /// </summary>
        /// <param name="id">The ID of the health issue to retrieve details for.</param>
        /// <returns>An <see cref="IActionResult"/> with the details of the health issue.</returns>
        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            var result = await healthIssueService.DetailsAsync(id);

            return Ok(result);
        }

        /// <summary>
        /// Edits an existing health issue.
        /// </summary>
        /// <param name="healthIssueEditModel">The model containing updated health issue details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success or failure of the operation.</returns>
        [HttpGet("Edit")]
        public async Task<IActionResult> Edit([FromQuery] HealthIssueEditModel healthIssueEditModel)
        {
            var result = await healthIssueService.EditAsync(healthIssueEditModel);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of health issues associated with a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose health issues are to be retrieved.</param>
        /// <returns>An <see cref="IActionResult"/> with a list of health issues for the user.</returns>
        [HttpGet("UserIssues")]
        public async Task<IActionResult> UserIssues(string userId)
        {
            var result = await healthIssueService.UserIssuesAsync(userId);

            return Ok(result);
        }
    }
}
