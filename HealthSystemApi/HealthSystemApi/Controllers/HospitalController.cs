using HealthSystemApi.Models.Hospital;
using HealthSystemApi.Services.AuthenticationService;
using HealthSystemApi.Services.HospitalService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] HospitalAddModel model)
        {
            var isAdmin = await authenticationService.IsAdministrator(model.Token);

            if (!isAdmin)
            {
                return Ok(new { Success = false });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await hospitalService.AddAsync(model);

            if (result)
            {
                return Ok(new { Success = result });
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

        [HttpGet("GetHospital")]
        public async Task<IActionResult> GetHospital([FromQuery] string token, int hospitalId)
        {
            var isAdmin = await authenticationService.IsAdministrator(token);

            if (!isAdmin)
            {
                return BadRequest(new { Success = false });
            }

            var hospital = await hospitalService.GetHospital(hospitalId);

            return Ok(new { ContactNumber = hospital.ContactNumber, Location = hospital.Location, Name = hospital.HospitalName, UserId = hospital.UserId });
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromForm] HospitalEditModel model)
        {
            var isAdmin = await authenticationService.IsAdministrator(model.Token ?? "");

            if (!isAdmin)
            {
                return BadRequest(new { Success = false });
            }

            var result = await hospitalService.EditAsync(model);

            return Ok(new { Success = result });
        }
    }
}
