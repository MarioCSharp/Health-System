using HealthSystem.Pharmacy.Models.Pharmacy;
using HealthSystem.Pharmacy.Services.PharmacyService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Pharmacy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PharmacyController : ControllerBase
    {
        private IPharmacyService pharmacyService;

        public PharmacyController(IPharmacyService pharmacyService)
        {
            this.pharmacyService = pharmacyService;
        }

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

        [HttpGet("All")]
        [Authorize]
        public async Task<IActionResult> All()
        {
            var result = await pharmacyService.AllAsync();

            return result is not null ? Ok(result) : BadRequest();
        }

        [HttpGet("Details")]
        [Authorize]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            var result = await pharmacyService.DetailsAsync();

            return result is not null ? Ok(result) : BadRequest();
        }

        [HttpGet("Delete")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await pharmacyService.DeleteAsync(id);

            return result ? Ok(result) : BadRequest();
        }

        [HttpPost("Edit")]
        [Authorize(Roles = "Administrator,PharmacyOwner")]
        public async Task<IActionResult> Edit([FromForm] PharmacyEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await pharmacyService.EditAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

            return result ? Ok(result) : BadRequest();
        }
    }
}
