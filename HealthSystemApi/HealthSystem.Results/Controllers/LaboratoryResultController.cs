using HealthSystem.Results.Models;
using HealthSystem.Results.Services.LaboratoryResultService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Results.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LaboratoryResultController : ControllerBase
    {
        private ILaboratoryResultService laboratoryResultService;

        public LaboratoryResultController(ILaboratoryResultService laboratoryResultService)
        {
            this.laboratoryResultService = laboratoryResultService;
        }

        [HttpPost("IssueResult")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> IssueResult([FromForm] IssueResultModel model)
        {
            var result = await laboratoryResultService.IssueResultAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier));

            return result.Item1 ? Ok(new { UserId = result.Item2, UserPass = result.Item3 }) : BadRequest();
        }

        [HttpGet("TryGetFile")]
        [Authorize]
        public async Task<IActionResult> TryGetFile([FromQuery] string id, string pass)
        {
            var result = await laboratoryResultService.GetFileAsync(id, pass);
             
            return Ok(result);
        }

        [HttpPost("AddFile")]
        public async Task<IActionResult> AddFile([FromForm] int resultId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected");

            var result = await laboratoryResultService.AddFileAsync(resultId, file);

            return result ? Ok() : BadRequest();
        }

        [HttpGet("GetDoctorResults")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorResults()
        {
            var result = await laboratoryResultService.GetResults(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

            return Ok(new { Results = result });
        }
    }
}
