using HealthSystemApi.Models.HealthIssue;
using HealthSystemApi.Services.HealthIssueService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystemApi.Controllers
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
        public async Task<IActionResult> Add([FromQuery] HealthIssueAddModel healthIssueAddModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await healthIssueService.AddAsync(healthIssueAddModel, User.FindFirstValue(ClaimTypes.NameIdentifier));

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
        public async Task<IActionResult> UserIssues()
        {
            var result = await healthIssueService.UserIssuesAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(result);
        }
    }
}
