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
        private IAppointmentService appointmentService; // Service for appointment-related operations

        public AppointmentController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        /// <summary>
        /// Retrieves the next appointments for the doctor based on their user ID.
        /// </summary>
        /// <returns>A list of upcoming appointments.</returns>
        [HttpGet("GetNextAppointmentsByDoctorUserId")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetNextAppointmentsByDoctorUserId()
        {
            var apps = await appointmentService.GetNextAppointmentsByDoctorUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(new { Appointments = apps });
        }

        /// <summary>
        /// Retrieves the past appointments for the doctor based on their user ID.
        /// </summary>
        /// <returns>A list of past appointments.</returns>
        [HttpGet("GetPastAppointmentsByDoctorUserId")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetPastAppointmentsByDoctorUserId()
        {
            var apps = await appointmentService.GetPastAppointmentsByDoctorUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(new { Appointments = apps });
        }

        /// <summary>
        /// Removes an appointment by ID.
        /// </summary>
        /// <param name="id">ID of the appointment to remove.</param>
        /// <returns>Result of the removal operation.</returns>
        [HttpGet("Remove")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            var result = await appointmentService.Remove(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(new { Success = result });
        }

        /// <summary>
        /// Adds a comment to an appointment.
        /// </summary>
        /// <param name="model">Comment model containing details.</param>
        /// <returns>Result of the add comment operation.</returns>
        [HttpPost("AddComment")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AddComment([FromForm] AppointmentCommentAddModel model)
        {
            var result = await appointmentService.AddComment(model, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(new { Success = result });
        }

        /// <summary>
        /// Issues a prescription for a specific appointment.
        /// </summary>
        /// <param name="model">Prescription model containing details.</param>
        /// <returns>The issued prescription as a file.</returns>
        [HttpPost("IssuePrescription")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> IssuePrescription([FromForm] PrescriptionModel model)
        {
            var result = await appointmentService.IssuePrescriptionAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier));

            var success = result.Item1;
            var file = result.Item2;

            if (success)
            {
                return File(file.OpenReadStream(), file.ContentType, file.FileName); // Return the prescription file
            }

            return BadRequest("Failed to issue prescription"); // Handle failure case
        }

        /// <summary>
        /// Checks if an appointment has a prescription issued.
        /// </summary>
        /// <param name="appointmentId">ID of the appointment.</param>
        /// <returns>The prescription file if available.</returns>
        [HttpGet("HasPrescription")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> HasPrescription([FromQuery] int appointmentId)
        {
            var result = await appointmentService.HasPrescriptionAsync(appointmentId);

            var success = result.Item1;
            var file = result.Item2;

            if (success)
            {
                return File(file.OpenReadStream(), file.ContentType, file.FileName); // Return the prescription file
            }

            return BadRequest(); // Handle failure case
        }

        /// <summary>
        /// Retrieves all appointments for a specific doctor.
        /// </summary>
        /// <param name="id">ID of the doctor.</param>
        /// <returns>A list of appointments for the doctor.</returns>
        [HttpGet("GetAppointments")]
        [Authorize(Roles = "Administrator,Director,Doctor")]
        public async Task<IActionResult> GetAppointments([FromQuery] int id)
        {
            var apps = await appointmentService.GetDoctorAppointments(id);
            return Ok(new { FullName = apps.Item1, Appointments = apps.Item2 });
        }

        /// <summary>
        /// Removes an appointment by ID (admin only).
        /// </summary>
        /// <param name="id">ID of the appointment to remove.</param>
        /// <returns>Result of the removal operation.</returns>
        [HttpGet("RemoveAppointment")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RemoveAppointment([FromQuery] int id)
        {
            var result = await appointmentService.RemoveAppointment(id);
            return Ok(new { Success = result });
        }

        /// <summary>
        /// Deletes all appointments for a specific doctor (admin or director only).
        /// </summary>
        /// <param name="doctorId">ID of the doctor.</param>
        /// <returns>Status of the deletion operation.</returns>
        [HttpGet("DeleteAllByDoctorId")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> DeleteAllByDoctorId([FromQuery] int doctorId)
        {
            await appointmentService.DeleteAllByDoctorId(doctorId);
            return Ok(); // Return success response
        }

        /// <summary>
        /// Retrieves all appointments for a specific user (admin only).
        /// </summary>
        /// <param name="userId">User ID of the patient.</param>
        /// <returns>A list of appointments for the user.</returns>
        [HttpGet("GetUserAppointments")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetUserAppointments([FromQuery] string userId)
        {
            var result = await appointmentService.GetUserAppointments(userId);
            return Ok(new { FullName = result.Item1, Appointments = result.Item2 });
        }

        /// <summary>
        /// Retrieves all prescriptions for a specific user.
        /// </summary>
        /// <param name="userId">User ID of the patient.</param>
        /// <returns>A list of prescriptions for the user.</returns>
        [HttpGet("GetUserPrescriptions")]
        public async Task<IActionResult> GetUserPrescriptions([FromQuery] string userId)
        {
            var result = await appointmentService.GetUserPrescriptions(userId);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves details of a specific appointment by ID.
        /// </summary>
        /// <param name="id">ID of the appointment.</param>
        /// <returns>Details of the appointment.</returns>
        [HttpGet("GetAppointment")]
        public async Task<IActionResult> GetAppointment([FromQuery] int id)
        {
            var result = await appointmentService.GetAppointment(id);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves the doctor ID associated with a specific appointment.
        /// </summary>
        /// <param name="id">ID of the appointment.</param>
        /// <returns>The ID of the doctor assigned to the appointment.</returns>
        [HttpGet("GetDoctorByAppointmentId")]
        public async Task<IActionResult> GetDoctorByAppointmentId([FromQuery] int id)
        {
            var result = await appointmentService.GetAppointment(id);
            return Ok(result.DoctorId); // Return the doctor ID
        }

        /// <summary>
        /// Retrieves the next appointments for the user.
        /// </summary>
        /// <returns>A list of upcoming appointments for the user.</returns>
        [HttpGet("GetUsersNextAppointments")]
        [Authorize]
        public async Task<IActionResult> GetUsersNextAppointments()
        {
            var nextAppointments = await appointmentService.GetUsersNextAppointments(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty);
            return Ok(nextAppointments);
        }
    }
}
