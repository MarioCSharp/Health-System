using HealthSystemApi.Models.Appointment;
using HealthSystemApi.Services.AppointmentService;
using HealthSystemApi.Services.AuthenticationService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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

        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment([FromForm] AppointmentCommentAddModel model)
        {
            var isDoctor = await authenticationService.IsDoctor(model.Token ?? "");

            if (!isDoctor)
            {
                return BadRequest();
            }

            var result = await appointmentService.AddComent(model);

            return Ok(new { Success = result });
        }

        [HttpPost("IssuePrescription")]
        public async Task<IActionResult> IssuePrescription([FromForm] PrescriptionModel model)
        {
            var isDoctor = await authenticationService.IsDoctor(model.Token ?? "");

            if (!isDoctor)
            {
                return BadRequest();
            }

            var result = await appointmentService.IssuePrescriptionAsync(model);

            var success = result.Item1;
            var file = result.Item2;

            if (success)
            {
                return File(file.OpenReadStream(), file.ContentType, file.FileName);
            }

            return BadRequest("Failed to issue prescription");
        }

        [HttpGet("HasPrescription")]
        public async Task<IActionResult> HasPrescription([FromQuery] string token, int appointmentId)
        {
            var isDoctor = await authenticationService.IsDoctor(token ?? "");

            if (!isDoctor)
            {
                return Unauthorized();
            }

            var result = await appointmentService.HasPrescriptionAsync(appointmentId);

            var success = result.Item1;
            var file = result.Item2;

            if (success)
            {
                return File(file.OpenReadStream(), file.ContentType, file.FileName);
            }

            return BadRequest();
        }

    }
}
