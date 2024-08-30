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

        public RecepcionistController(IRecepcionistService recepcionistService)
        {
            this.recepcionistService = recepcionistService;
        }

        [HttpPost("Add")]
        [Authorize(Roles = "Director")]
        public async Task<IActionResult> Add([FromForm] string userId, int hospitalId, string name)
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

        [HttpGet("GetHospitalId")]
        public async Task<IActionResult> GetHospitalId([FromQuery] string userId)
        {
            var result = await recepcionistService.GetHospitalIdAsync(userId);

            return Ok(result);
        }

        [HttpGet("GetHospitalAndUserId")]
        [Authorize(Roles = "Recepcionist")]
        public async Task<IActionResult> GetHospitalAndUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var hospitalId = await recepcionistService.GetHospitalIdAsync(userId);

            return Ok(new {UserId = userId, HospitalId = hospitalId });
        }

        [HttpGet("GetMyRecepcionists")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> GetMyRecepcionists()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var recepcionists = await recepcionistService.GetMyRecepcionists(userId);

            return Ok(new { Recepcionists = recepcionists});
        }

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
