using HealthSystem.HealthCare.Models;
using HealthSystem.HealthCare.Services.HealthIssueService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.HealthCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthIssueController : ControllerBase
    {
        private IHealthIssueService healthIssueService;
        public HealthIssueController(IHealthIssueService healthIssueService)
        {
            this.healthIssueService = healthIssueService;
        }

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

        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            var result = await healthIssueService.DetailsAsync(id);

            return Ok(result);
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit([FromQuery] HealthIssueEditModel healthIssueEditModel)
        {
            var result = await healthIssueService.EditAsync(healthIssueEditModel);

            return Ok(result);
        }

        [HttpGet("UserIssues")]
        public async Task<IActionResult> UserIssues(string userId)
        {
            var result = await healthIssueService.UserIssuesAsync(userId);

            return Ok(result);
        }
    }
}
