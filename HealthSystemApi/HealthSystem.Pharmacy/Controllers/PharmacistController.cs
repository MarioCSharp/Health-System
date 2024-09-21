using HealthSystem.Pharmacy.Models.Pharmacist;
using HealthSystem.Pharmacy.Services.PharmacistService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Pharmacy.Controllers
{
    /// <summary>
    /// API controller responsible for managing pharmacist operations in the pharmacy system.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PharmacistController : ControllerBase
    {
        private IPharmacistService pharmacistService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PharmacistController"/> class.
        /// </summary>
        /// <param name="pharmacistService">The <see cref="IPharmacistService"/> responsible for pharmacist-related operations.</param>
        public PharmacistController(IPharmacistService pharmacistService)
        {
            this.pharmacistService = pharmacistService;
        }

        /// <summary>
        /// Adds a new pharmacist to the pharmacy.
        /// </summary>
        /// <param name="model">The <see cref="PharmacistAddModel"/> containing the pharmacist details to add.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost("Add")]
        [Authorize(Roles = "Administrator,PharmacyOwner")]
        public async Task<IActionResult> Add([FromForm] PharmacistAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var authHeader = Request.Headers["Authorization"].ToString();
            var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var result = await pharmacistService.AddAsync(model, User.IsInRole("Administrator") ? "Administrator" : userId, token);

            return result ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Retrieves all pharmacists in a specified pharmacy.
        /// </summary>
        /// <param name="pharmacyId">The ID of the pharmacy to retrieve pharmacists for.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of pharmacists.</returns>
        [HttpGet("AllInPharmacy")]
        [Authorize(Roles = "Administrator,PharmacyOwner")]
        public async Task<IActionResult> AllInPharmacy([FromQuery] int pharmacyId)
        {
            var result = await pharmacistService.AllByPharmacyId(pharmacyId);

            return result is not null ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Deletes a specified pharmacist from the pharmacy.
        /// </summary>
        /// <param name="pharmacistId">The ID of the pharmacist to delete.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the deletion operation.</returns>
        [HttpGet("Delete")]
        [Authorize(Roles = "Administrator,PharmacyOwner")]
        public async Task<IActionResult> Delete([FromQuery] int pharmacistId)
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var result = await pharmacistService.DeleteAsync(pharmacistId, token);

            return result ? Ok(result) : BadRequest();
        }
    }
}
