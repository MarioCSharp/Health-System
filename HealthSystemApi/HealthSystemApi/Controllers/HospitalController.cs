using HealthSystemApi.Models.Hospital;
using HealthSystemApi.Services.HospitalService;
using Microsoft.AspNetCore.Mvc;

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

        [HttpGet("All")]
        public async Task<IActionResult> All()
        {
            var hospitals = await hospitalService.AllAsync();

            return Ok(hospitals);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            var result = await hospitalService.HospitalDetails(id);

            return Ok(result);
        }
    }
}
