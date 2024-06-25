using HealthSystemApi.Models.Hospital;
using HealthSystemApi.Services.HospitalService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HospitalController : ControllerBase
    {
        private IHospitalService hospitalService;
        public HospitalController(IHospitalService hospitalService)
        {
            this.hospitalService = hospitalService;
        }
        [HttpGet("Add")]
        public async Task<IActionResult> Add([FromQuery] HospitalAddModel model)
        {
            var result = await hospitalService.AddAsync(model);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("Remove")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            var result = await hospitalService.RemoveAsync(id);

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
