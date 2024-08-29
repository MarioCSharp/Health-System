using HealthSystem.Admins.Services.RecepcionistService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Add([FromForm] string userId, int hospitalId, string name)
        {
            var result = await recepcionistService.AddAsync(userId, hospitalId, name);

            return Ok(result);
        }

        [HttpGet("GetHospitalId")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> GetHospitalId([FromQuery] string userId)
        {
            var result = await recepcionistService.GetHospitalIdAsync(userId);

            return Ok(result);
        }
    }
}
