using HealthSystem.Booking.Models;
using HealthSystem.Booking.Services.AppointmentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private IAppointmentService appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        [HttpGet("GetNextAppointmentsByDoctorUserId")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetNextAppointmentsByDoctorUserId()
        {
            var apps = await appointmentService.GetNextAppointmentsByDoctorUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(new { Appointments = apps });
        }

        [HttpGet("GetPastAppointmentsByDoctorUserId")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetPastAppointmentsByDoctorUserId()
        {
            var apps = await appointmentService.GetPastAppointmentsByDoctorUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(new { Appointments = apps });
        }

        [HttpGet("Remove")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            var result = await appointmentService.Remove(id, User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(new { Success = result });
        }

        [HttpPost("AddComment")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AddComment([FromForm] AppointmentCommentAddModel model)
        {
            var result = await appointmentService.AddComment(model, User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(new { Success = result });
        }

        [HttpPost("IssuePrescription")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> IssuePrescription([FromForm] PrescriptionModel model)
        {
            var result = await appointmentService.IssuePrescriptionAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier));

            var success = result.Item1;
            var file = result.Item2;

            if (success)
            {
                return File(file.OpenReadStream(), file.ContentType, file.FileName);
            }

            return BadRequest("Failed to issue prescription");
        }

        [HttpGet("HasPrescription")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> HasPrescription([FromQuery] int appointmentId)
        {
            var result = await appointmentService.HasPrescriptionAsync(appointmentId);

            var success = result.Item1;
            var file = result.Item2;

            if (success)
            {
                return File(file.OpenReadStream(), file.ContentType, file.FileName);
            }

            return BadRequest();
        }

        [HttpGet("GetAppointments")]
        [Authorize(Roles = "Administrator,Director,Doctor")]
        public async Task<IActionResult> GetAppointments([FromQuery] int id)
        {
            var apps = await appointmentService.GetDoctorAppointments(id);

            return Ok(new { FullName = apps.Item1, Appointments = apps.Item2 });
        }

        [HttpGet("RemoveAppointment")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RemoveAppointment([FromQuery] int id)
        {
            var result = await appointmentService.RemoveAppointment(id);

            return Ok(new { Success = result });
        }

        [HttpGet("DeleteAllByDoctorId")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> DeleteAllByDoctorId([FromQuery] int doctorId)
        {
             await appointmentService.DeleteAllByDoctorId(doctorId);

            return Ok();
        }

        [HttpGet("GetUserAppointments")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetUserAppointments([FromQuery] string userId)
        {
            var result = await appointmentService.GetUserAppointments(userId);

            return Ok(new { FullName = result.Item1, Appointments = result.Item2 });
        }

        [HttpGet("GetUserPrescriptions")]
        public async Task<IActionResult> GetUserPrescriptions([FromQuery] string userId)
        {
            var result = await appointmentService.GetUserPrescriptions(userId);

            return Ok(result);
        }

        [HttpGet("GetAppointment")]
        public async Task<IActionResult> GetAppointment([FromQuery] int id)
        {
            var result = await appointmentService.GetAppointment(id);

            return Ok(result);
        }

        [HttpGet("GetDoctorByAppointmentId")]
        public async Task<IActionResult> GetDoctorByAppointmentId([FromQuery] int id)
        {
            var result = await appointmentService.GetAppointment(id);

            return Ok(result.DoctorId);
        }

        [HttpGet("GetUsersNextAppointments")]
        [Authorize]
        public async Task<IActionResult> GetUsersNextAppointments()
        {
            var nextAppointmets = await appointmentService.GetUsersNextAppointments(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);

            return Ok(nextAppointmets);
        }
    }
}
