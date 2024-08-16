using HealthSystemApi.Services.AppointmentService;
using HealthSystemApi.Services.AuthenticationService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private IAppointmentService appointmentService;
        private IAuthenticationService authenticationService;

        public AppointmentController(IAppointmentService appointmentService,
                                     IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
            this.appointmentService = appointmentService;
        }

        [HttpGet("GetNextAppointmentsByDoctorUserId")]
        public async Task<IActionResult> GetNextAppointmentsByDoctorUserId([FromQuery] string token)
        {
            var isDoctor = await authenticationService.IsDoctor(token);

            if (!isDoctor)
            {
                return BadRequest();
            }

            var apps = await appointmentService.GetNextAppointmentsByDoctorUserId(token);

            return Ok(new { Appointments = apps });
        }

        [HttpGet("GetPastAppointmentsByDoctorUserId")]
        public async Task<IActionResult> GetPastAppointmentsByDoctorUserId([FromQuery] string token)
        {
            var isDoctor = await authenticationService.IsDoctor(token);

            if (!isDoctor)
            {
                return BadRequest();
            }

            var apps = await appointmentService.GetPastAppointmentsByDoctorUserId(token);

            return Ok(new { Appointments = apps });
        }

        [HttpGet("Remove")]
        public async Task<IActionResult> Remove([FromQuery] string token, int id)
        {
            var isDoctor = await authenticationService.IsDoctor(token);

            if (!isDoctor)
            {
                return BadRequest();
            }

            var result = await appointmentService.Remove(id);

            return Ok(new { Success = result });
        }
    }
}
