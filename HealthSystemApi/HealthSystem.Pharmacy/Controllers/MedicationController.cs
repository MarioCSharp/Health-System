using HealthSystem.Pharmacy.Models.Medication;
using HealthSystem.Pharmacy.Services.MedicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Pharmacy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicationController : ControllerBase
    {
        private IMedicationService medicationService;

        public MedicationController(IMedicationService medicationService)
        {
            this.medicationService = medicationService;
        }

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

        [HttpGet("AddQuantity")]
        [Authorize(Roles = "PharmacyOwner,Pharmacist")]
        public async Task<IActionResult> AddQuantity([FromQuery] int medicationId, int quantity)
        {
            var result = await medicationService.AddQuantityAsync(medicationId, quantity);

            return result ? Ok(result) : BadRequest();
        }

        [HttpGet("AllInPharmacy")]
        [Authorize]
        public async Task<IActionResult> AllInPharmacy([FromQuery] int pharmacyId)
        {
            var result = await medicationService.AllInPharmacyAsync(pharmacyId);

            return Ok(result);
        }

        [HttpGet("Delete")]
        [Authorize(Roles = "PharmacyOwner,Pharmacist")]
        public async Task<IActionResult> Delete([FromQuery] int medicationId)
        {
            var result = await medicationService.DeleteAsync(medicationId);

            return Ok(result);
        }

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

        [HttpGet("GetMyMedications")]
        [Authorize(Roles = "PharmacyOwner,Pharmacist")]
        public async Task<IActionResult> GetMyMedications()
        {
            var result = await medicationService.GetMedications(User.FindFirstValue(ClaimTypes.NameIdentifier), User.IsInRole("Pharmacist") ? "Pharmacist" : "PharmacyOwner");

            return Ok(result);
        }
    }
}
