using HealthSystemApi.Models.Hospital;
using HealthSystemApi.Services.AuthenticationService;
using HealthSystemApi.Services.HospitalService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HospitalController : ControllerBase
    {
        private IHospitalService hospitalService;
        private IAuthenticationService authenticationService;

        public HospitalController(IHospitalService hospitalService,
                                  IAuthenticationService authenticationService)
        {
            this.hospitalService = hospitalService;
            this.authenticationService = authenticationService;
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
        public async Task<IActionResult> Remove([FromQuery] int id, string token)
        {
            var isAdmin = await authenticationService.IsAdministrator(token);

            if (!isAdmin)
            {
                return Ok(new { Success = false });
            }

            var result = await hospitalService.RemoveAsync(id);

            if (result)
            {
                return Ok(new { Success = true });
            }

            return Ok(new { Success = false });
        }

        [HttpGet("All")]
        public async Task<IActionResult> All()
        {
            var hospitals = await hospitalService.AllAsync();

            return Ok(new { Hospitals = hospitals});
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            var result = await hospitalService.HospitalDetails(id);

            return Ok(result);
        }

        [HttpGet("GetDoctors")]
        public async Task<IActionResult> GetDoctors([FromQuery] int id, string token)
        {
            var isAdmin = await authenticationService.IsAdministrator(token);

            if (!isAdmin)
            {
                return BadRequest(new { Success = false });
            }

            var doctors = await hospitalService.GetDoctorsAsync(id);

            return Ok(new { Doctors = doctors });
        }
    }
}
