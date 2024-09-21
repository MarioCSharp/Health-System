using HealthSystem.Pharmacy.Models.Medication;
using HealthSystem.Pharmacy.Services.MedicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Pharmacy.Controllers
{
    /// <summary>
    /// API controller responsible for managing medication operations in the pharmacy system.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MedicationController : ControllerBase
    {
        private IMedicationService medicationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicationController"/> class.
        /// </summary>
        /// <param name="medicationService">The <see cref="IMedicationService"/> responsible for medication-related operations.</param>
        public MedicationController(IMedicationService medicationService)
        {
            this.medicationService = medicationService;
        }

        /// <summary>
        /// Adds a new medication to the pharmacy.
        /// </summary>
        /// <param name="model">The <see cref="MedicationAddModel"/> containing the medication details to add.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost("Add")]
        [Authorize(Roles = "PharmacyOwner,Pharmacist")]
        public async Task<IActionResult> Add([FromForm] MedicationAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await medicationService.AddAsync(model);

            return result ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Adds a specified quantity to an existing medication.
        /// </summary>
        /// <param name="medicationId">The ID of the medication to update.</param>
        /// <param name="quantity">The quantity to add.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpGet("AddQuantity")]
        [Authorize(Roles = "PharmacyOwner,Pharmacist")]
        public async Task<IActionResult> AddQuantity([FromQuery] int medicationId, int quantity)
        {
            var result = await medicationService.AddQuantityAsync(medicationId, quantity);

            return result ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Retrieves all medications available in a specific pharmacy.
        /// </summary>
        /// <param name="pharmacyId">The ID of the pharmacy to retrieve medications for.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of medications.</returns>
        [HttpGet("AllInPharmacy")]
        [Authorize]
        public async Task<IActionResult> AllInPharmacy([FromQuery] int pharmacyId)
        {
            var result = await medicationService.AllInPharmacyAsync(pharmacyId);

            return Ok(result);
        }

        /// <summary>
        /// Deletes a specified medication from the pharmacy.
        /// </summary>
        /// <param name="medicationId">The ID of the medication to delete.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the deletion operation.</returns>
        [HttpGet("Delete")]
        [Authorize(Roles = "PharmacyOwner,Pharmacist")]
        public async Task<IActionResult> Delete([FromQuery] int medicationId)
        {
            var result = await medicationService.DeleteAsync(medicationId);

            return Ok(result);
        }

        /// <summary>
        /// Edits an existing medication's details.
        /// </summary>
        /// <param name="model">The <see cref="MedicationEditModel"/> containing the updated medication details.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the edit operation.</returns>
        [HttpPost("Edit")]
        [Authorize(Roles = "PharmacyOwner,Pharmacist")]
        public async Task<IActionResult> Edit([FromForm] MedicationEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await medicationService.EditAsync(model);

            return result ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Retrieves the medications belonging to the current user, either a pharmacy owner or a pharmacist.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of the user's medications.</returns>
        [HttpGet("GetMyMedications")]
        [Authorize(Roles = "PharmacyOwner,Pharmacist")]
        public async Task<IActionResult> GetMyMedications()
        {
            var result = await medicationService.GetMedications(User.FindFirstValue(ClaimTypes.NameIdentifier), User.IsInRole("Pharmacist") ? "Pharmacist" : "PharmacyOwner");

            return Ok(result);
        }
    }
}
