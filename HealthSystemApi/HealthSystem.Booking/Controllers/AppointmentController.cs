﻿using HealthSystem.Booking.Models;
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
            var result = await appointmentService.Remove(id);

            return Ok(new { Success = result });
        }

        [HttpPost("AddComment")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AddComment([FromForm] AppointmentCommentAddModel model)
        {
            var result = await appointmentService.AddComent(model);

            return Ok(new { Success = result });
        }

        [HttpPost("IssuePrescription")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> IssuePrescription([FromForm] PrescriptionModel model)
        {
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
        [Authorize(Roles = "Administrator,Director")]
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
    }
}
