using HealthSystem.Admins.Models;
using HealthSystem.Admins.Services.DoctorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Admins.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private IDoctorService doctorService;
        public DoctorController(IDoctorService doctorService)
        {
            this.doctorService = doctorService;
        }

        [HttpGet("Add")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> Add([FromQuery] DoctorAddModel model)
        {
            var result = await doctorService.AddAsync(model, User.IsInRole("Director") ? User.FindFirstValue(ClaimTypes.NameIdentifier) : string.Empty);

            return result ? Ok(new { Success = result }) : BadRequest();
        }

        [HttpGet("Remove")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            var result = await doctorService.RemoveAsync(id, User.IsInRole("Director") ? User.FindFirstValue(ClaimTypes.NameIdentifier) : string.Empty);

            return result ? Ok() : BadRequest();
        }

        [HttpGet("All")]
        public async Task<IActionResult> All([FromQuery] int id)
        {
            var result = await doctorService.GetAllAsync(id);

            return Ok(result);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            return Ok(await doctorService.GetDetailsAsync(id));
        }

        [HttpGet("GetDoctor")]
        public async Task<IActionResult> GetDoctor([FromQuery] int id)
        {
            return Ok(await doctorService.GetDoctor(id));
        }

        [HttpGet("Edit")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> Edit([FromQuery] DoctorDetailsModel model)
        {
            await doctorService.Edit(model, User.IsInRole("Director") ? User.FindFirstValue(ClaimTypes.NameIdentifier) : string.Empty);

            return Ok();
        }

        [HttpGet("GetDoctorByUserId")]
        public async Task<IActionResult> GetDoctorByUserId(string userId)
        {
            return Ok(await doctorService.GetDoctorByUserId(userId));
        }

        [HttpGet("AllByDirector")]
        [Authorize(Roles = "Director")]
        public async Task<IActionResult> AllByDirector()
        {
            var result = await doctorService.GetAllDoctorsByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(new { Doctors = result });
        }

        [HttpGet("HospitalIdByDirector")]
        [Authorize(Roles = "Director")]
        public async Task<IActionResult> HospitalIdByDirector()
        {
            var result = await doctorService.HospitalIdByDirector(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(new { HospitalId = result });
        }

        [HttpGet("GetDoctorId")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorId()
        {
            var result = await doctorService.GetDoctorByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(new { Id = result.Id });
        }

        [HttpGet("AddRating")]
        [Authorize]
        public async Task<IActionResult> AddRating([FromQuery] float rating, string comment, int doctorId, int appointmentId)
        {
            var result = await doctorService.AddRating(rating, comment, doctorId, appointmentId, User.FindFirstValue(ClaimTypes.NameIdentifier));

            return result ? Ok(true) : BadRequest();
        }

        [HttpGet("HasRating")]
        public async Task<IActionResult> HasRating([FromQuery] int appointmentId)
        {
            return Ok(await doctorService.AppointmentHasRating(appointmentId));
        }

        [HttpGet("GetTopDoctorsWithSpecialization")]
        public async Task<IActionResult> GetTopDoctorsWithSpecialization([FromQuery] string specialization, int top)
        {
            return Ok(await doctorService.GetTopDoctorsWithSpecialization(specialization, top));
        }
    }
}