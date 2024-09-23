using HealthSystem.Results.Models;
using HealthSystem.Results.Services.LaboratoryResultService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Results.Controllers
{
    /// <summary>
    /// API controller for handling laboratory result-related actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LaboratoryResultController : ControllerBase
    {
        private ILaboratoryResultService laboratoryResultService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LaboratoryResultController"/> class.
        /// </summary>
        /// <param name="laboratoryResultService">Service for managing laboratory results.</param>
        public LaboratoryResultController(ILaboratoryResultService laboratoryResultService)
        {
            this.laboratoryResultService = laboratoryResultService;
        }

        /// <summary>
        /// Issues a laboratory result by a doctor.
        /// </summary>
        /// <param name="model">The model containing result issue details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating success or failure.</returns>
        [HttpPost("IssueResult")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> IssueResult([FromForm] IssueResultModel model)
        {
            var result = await laboratoryResultService.IssueResultAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier));

            return result.Item1 ? Ok(new { UserId = result.Item2, UserPass = result.Item3 }) : BadRequest();
        }

        /// <summary>
        /// Attempts to retrieve a laboratory result file.
        /// </summary>
        /// <param name="id">The ID of the laboratory result file.</param>
        /// <param name="pass">The password for accessing the file.</param>
        /// <returns>An <see cref="IActionResult"/> containing the file or indicating failure.</returns>
        [HttpGet("TryGetFile")]
        [Authorize]
        public async Task<IActionResult> TryGetFile([FromQuery] string id, string pass)
        {
            var result = await laboratoryResultService.GetFileAsync(id, pass);

            return Ok(result);
        }

        /// <summary>
        /// Adds a file to a specific laboratory result.
        /// </summary>
        /// <param name="resultId">The ID of the laboratory result to which the file is being added.</param>
        /// <param name="file">The file to be uploaded.</param>
        /// <returns>An <see cref="IActionResult"/> indicating success or failure.</returns>
        [HttpPost("AddFile")]
        public async Task<IActionResult> AddFile([FromForm] int resultId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected");

            var result = await laboratoryResultService.AddFileAsync(resultId, file);

            return result ? Ok() : BadRequest();
        }

        /// <summary>
        /// Retrieves all laboratory results for the current doctor.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the doctor's laboratory results.</returns>
        [HttpGet("GetDoctorResults")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorResults()
        {
            var result = await laboratoryResultService.GetResults(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

            return Ok(new { Results = result });
        }
    }
}
