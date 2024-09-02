using HealthSystem.Pharmacy.Models.Pharmacist;
using HealthSystem.Pharmacy.Services.PharmacistService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Pharmacy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PharmacistController : ControllerBase
    {
        private IPharmacistService pharmacistService;

        public PharmacistController(IPharmacistService pharmacistService)
        {
            this.pharmacistService = pharmacistService;
        }

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

        [HttpGet("AllInPharmacy")]
        [Authorize(Roles = "Administrator,PharmacyOwner")]
        public async Task<IActionResult> AllInPharmacy([FromQuery] int pharmacyId)
        {
            var result = await pharmacistService.AllByPharmacyId(pharmacyId);

            return result is not null ? Ok(true) : BadRequest();
        }

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
