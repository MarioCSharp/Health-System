using HealthSystemApi.Data;
using HealthSystemApi.Models.Doctor;
using HealthSystemApi.Services.AuthenticationService;
using HealthSystemApi.Services.DoctorService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private ApplicationDbContext context;
        private IDoctorService doctorService;
        private IAuthenticationService authenticationService;

        public DoctorController(ApplicationDbContext context,
                                IDoctorService doctorService,
                                IAuthenticationService authenticationService)
        {
            this.context = context;
            this.doctorService = doctorService;
            this.authenticationService = authenticationService;
        }

        [HttpGet("Add")]
        public async Task<IActionResult> Add([FromQuery] DoctorAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await doctorService.AddAsync(model);

            return result ? Ok() : BadRequest();
        }

        [HttpGet("Remove")]
        public async Task<IActionResult> Remove([FromQuery] int id, string token)
        {
            var isAdmin = await authenticationService.IsAdministrator(token);

            if (!isAdmin) 
            { 
                return BadRequest();
            }

            var result = await doctorService.RemoveAsync(id);

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
        public async Task<IActionResult> Edit([FromQuery] DoctorDetailsModel model)
        {
            await doctorService.Edit(model);

            return Ok();
        }

        [HttpGet("GetAppointments")]
        public async Task<IActionResult> GetAppointments([FromQuery] int id, string token)
        {
            var isAdmin = await authenticationService.IsAdministrator(token);

            if (!isAdmin)
            {
                return BadRequest();
            }

            var apps = await doctorService.GetDoctorAppointments(id);

            return Ok(new { FullName = apps.Item1, Appointments = apps.Item2 });
        }

        [HttpGet("RemoveAppointment")]
        public async Task<IActionResult> RemoveAppointment([FromQuery] int id, string token)
        {
            var isAdmin = await authenticationService.IsAdministrator(token);

            if (!isAdmin)
            {
                return BadRequest();
            }

            var result = await doctorService.RemoveAppointment(id);

            return Ok(new { Success = result });
        }
    }
}
