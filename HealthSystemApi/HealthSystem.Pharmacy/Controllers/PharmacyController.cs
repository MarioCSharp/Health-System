using HealthSystem.Pharmacy.Models.Pharmacy;
using HealthSystem.Pharmacy.Services.PharmacyService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Pharmacy.Controllers
{
    /// <summary>
    /// API controller responsible for managing pharmacy-related operations in the pharmacy system.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PharmacyController : ControllerBase
    {
        private IPharmacyService pharmacyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PharmacyController"/> class.
        /// </summary>
        /// <param name="pharmacyService">The <see cref="IPharmacyService"/> responsible for pharmacy operations.</param>
        public PharmacyController(IPharmacyService pharmacyService)
        {
            this.pharmacyService = pharmacyService;
        }

        /// <summary>
        /// Adds a new pharmacy.
        /// </summary>
        /// <param name="model">The <see cref="PharmacyAddModel"/> containing the details of the pharmacy to add.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the add operation.</returns>
        [HttpPost("Add")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Add([FromForm] PharmacyAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authHeader = Request.Headers["Authorization"].ToString();
            var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var result = await pharmacyService.AddAsync(model, token);

            return result ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Retrieves all pharmacies.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of all pharmacies.</returns>
        [HttpGet("All")]
        [Authorize]
        public async Task<IActionResult> All()
        {
            var result = await pharmacyService.AllAsync();

            return result is not null ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Retrieves the details of a specific pharmacy.
        /// </summary>
        /// <param name="id">The ID of the pharmacy to retrieve details for.</param>
        /// <returns>An <see cref="IActionResult"/> containing the pharmacy details.</returns>
        [HttpGet("Details")]
        [Authorize]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            var result = await pharmacyService.DetailsAsync(id);

            return result is not null ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Deletes a specific pharmacy.
        /// </summary>
        /// <param name="id">The ID of the pharmacy to delete.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the delete operation.</returns>
        [HttpGet("Delete")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : string.Empty;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var result = await pharmacyService.DeleteAsync(id, token);

            return result ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Edits a specific pharmacy's details.
        /// </summary>
        /// <param name="model">The <see cref="PharmacyEditModel"/> containing the updated pharmacy details.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the edit operation.</returns>
        [HttpPost("Edit")]
        [Authorize(Roles = "Administrator,PharmacyOwner")]
        public async Task<IActionResult> Edit([FromForm] PharmacyEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await pharmacyService.EditAsync(model, User.IsInRole("Administrator") ? "Administrator" : userId ?? string.Empty);

            return result ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Retrieves the pharmacy associated with the current pharmacy owner.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the pharmacy details.</returns>
        [HttpGet("GetMyPharmacy")]
        [Authorize(Roles = "PharmacyOwner")]
        public async Task<IActionResult> GetMyPharmacy()
        {
            return Ok(await pharmacyService
                .GetPharmacyByUserIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier),
                User.IsInRole("PharmacyOwner") ? "PharmacyOwner" : "Pharmacist"));
        }

        /// <summary>
        /// Retrieves the pharmacy ID associated with the current pharmacy owner or pharmacist.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the pharmacy ID.</returns>
        [HttpGet("GetMyPharmacyId")]
        [Authorize(Roles = "PharmacyOwner,Pharmacist")]
        public async Task<IActionResult> GetMyPharmacyId()
        {
            var pharmacy = await pharmacyService
                .GetPharmacyByUserIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier),
                User.IsInRole("PharmacyOwner") ? "PharmacyOwner" : "Pharmacist");

            return Ok(pharmacy.Id);
        }
    }
}
