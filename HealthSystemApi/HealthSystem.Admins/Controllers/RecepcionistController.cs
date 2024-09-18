using HealthSystem.Admins.Services.RecepcionistService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Admins.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecepcionistController : ControllerBase
    {
        private IRecepcionistService recepcionistService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecepcionistController"/> class.
        /// </summary>
        /// <param name="recepcionistService">The service for handling receptionist-related operations.</param>
        public RecepcionistController(IRecepcionistService recepcionistService)
        {
            this.recepcionistService = recepcionistService;
        }

        /// <summary>
        /// Adds a new receptionist to a specific hospital.
        /// </summary>
        /// <param name="userId">The ID of the user to add as a receptionist.</param>
        /// <param name="hospitalId">The ID of the hospital where the receptionist will work.</param>
        /// <param name="name">The name of the receptionist.</param>
        /// <returns>An IActionResult indicating the result of the addition.</returns>
        [HttpPost("Add")]
        [Authorize(Roles = "Director")]
        public async Task<IActionResult> Add([FromForm] string userId, [FromForm] int hospitalId, [FromForm] string name)
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var result = await recepcionistService.AddAsync(userId, hospitalId, name, token);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves the hospital ID associated with a given user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose hospital ID is to be retrieved.</param>
        /// <returns>An IActionResult containing the hospital ID.</returns>
        [HttpGet("GetHospitalId")]
        public async Task<IActionResult> GetHospitalId([FromQuery] string userId)
        {
            var result = await recepcionistService.GetHospitalIdAsync(userId);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves the hospital ID and user ID for the currently logged-in receptionist.
        /// </summary>
        /// <returns>An IActionResult containing both the user ID and hospital ID.</returns>
        [HttpGet("GetHospitalAndUserId")]
        [Authorize(Roles = "Recepcionist")]
        public async Task<IActionResult> GetHospitalAndUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var hospitalId = await recepcionistService.GetHospitalIdAsync(userId);

            return Ok(new { UserId = userId, HospitalId = hospitalId });
        }

        /// <summary>
        /// Retrieves all receptionists associated with the currently logged-in administrator or director.
        /// </summary>
        /// <returns>An IActionResult containing a list of receptionists.</returns>
        [HttpGet("GetMyRecepcionists")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> GetMyRecepcionists()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var recepcionists = await recepcionistService.GetMyRecepcionists(userId);

            return Ok(new { Recepcionists = recepcionists });
        }

        /// <summary>
        /// Deletes a receptionist by ID.
        /// </summary>
        /// <param name="id">The ID of the receptionist to delete.</param>
        /// <returns>An IActionResult indicating the result of the deletion.</returns>
        [HttpGet("Delete")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            await recepcionistService.Delete(id, token);

            return Ok();
        }
    }
}
