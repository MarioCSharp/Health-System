using HealthSystem.Admins.Models;
using HealthSystem.Admins.Services.HospitalService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace HealthSystem.Admins.Controllers
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

        [HttpPost("Add")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Add([FromForm] HospitalAddModel model)
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

            var result = await hospitalService.AddAsync(model, token);

            if (result)
            {
                return Ok(new { Success = result });
            }

            return BadRequest();
        }

        [HttpGet("Remove")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            var token = authHeader?.StartsWith("Bearer ") == true ? authHeader.Substring("Bearer ".Length).Trim() : null;

            var result = await hospitalService.RemoveAsync(id, token);

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

            return Ok(new { Hospitals = hospitals });
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            var result = await hospitalService.HospitalDetails(id);

            return Ok(result);
        }

        [HttpGet("GetDoctors")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> GetDoctors([FromQuery] int id)
        {
            var doctors = await hospitalService.GetDoctorsAsync(id, User.IsInRole("Director") ? User.FindFirstValue(ClaimTypes.NameIdentifier) : string.Empty);

            return Ok(new { Doctors = doctors });
        }

        [HttpGet("GetHospital")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> GetHospital([FromQuery] int hospitalId)
        {
            var hospital = await hospitalService.GetHospital(hospitalId, User.IsInRole("Director") ? User.FindFirstValue(ClaimTypes.NameIdentifier) : string.Empty);

            return Ok(new { ContactNumber = hospital.ContactNumber, Location = hospital.Location, Name = hospital.HospitalName, UserId = hospital.UserId });
        }

        [HttpPost("Edit")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> Edit([FromForm] HospitalEditModel model)
        {
            var result = await hospitalService.EditAsync(model, User.IsInRole("Director") ? User.FindFirstValue(ClaimTypes.NameIdentifier) : string.Empty);

            return Ok(new { Success = result });
        }

        [HttpGet("GetDirectorHospital")]
        [Authorize(Roles = "Director")]
        public async Task<IActionResult> GetDirectorHospital([FromQuery] string token)
        {
            var hospital = await hospitalService.GetHospitalByToken(token);

            return Ok(new { Hospital = hospital });
        }

        [HttpGet("GetDirectorHospitalId")]
        [Authorize(Roles = "Director")]
        public async Task<IActionResult> GetDirectorHospitalId()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            var token = authHeader?.StartsWith("Bearer ") == true ? authHeader.Substring("Bearer ".Length).Trim() : null;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var hospital = await hospitalService.GetHospitalByToken(token);

            return Ok(new { HospitalId = hospital.Id });
        }
    }
}
