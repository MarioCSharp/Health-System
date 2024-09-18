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

        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorController"/> class.
        /// </summary>
        /// <param name="doctorService">The service for handling doctor-related operations.</param>
        public DoctorController(IDoctorService doctorService)
        {
            this.doctorService = doctorService;
        }

        /// <summary>
        /// Adds a new doctor.
        /// </summary>
        /// <param name="model">The model containing information about the doctor to add.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpGet("Add")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> Add([FromQuery] DoctorAddModel model)
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

            var result = await doctorService.AddAsync(model, User.IsInRole("Director") ? User.FindFirstValue(ClaimTypes.NameIdentifier) : string.Empty, token);

            return result ? Ok(new { Success = result }) : BadRequest();
        }

        /// <summary>
        /// Removes a doctor by ID.
        /// </summary>
        /// <param name="id">The ID of the doctor to remove.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpGet("Remove")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            var token = authHeader?.StartsWith("Bearer ") == true ? authHeader.Substring("Bearer ".Length).Trim() : null;

            var result = await doctorService.RemoveAsync(id, User.IsInRole("Director") ? User.FindFirstValue(ClaimTypes.NameIdentifier) : string.Empty, token);

            return result ? Ok() : BadRequest();
        }

        /// <summary>
        /// Retrieves all doctors.
        /// </summary>
        /// <param name="id">The ID of the hospital to get doctors from.</param>
        /// <returns>An IActionResult containing the list of doctors.</returns>
        [HttpGet("All")]
        public async Task<IActionResult> All([FromQuery] int id)
        {
            var result = await doctorService.GetAllAsync(id);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves details of a specific doctor.
        /// </summary>
        /// <param name="id">The ID of the doctor to get details for.</param>
        /// <returns>An IActionResult containing the doctor's details.</returns>
        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            return Ok(await doctorService.GetDetailsAsync(id));
        }

        /// <summary>
        /// Retrieves a doctor by ID.
        /// </summary>
        /// <param name="id">The ID of the doctor to retrieve.</param>
        /// <returns>An IActionResult containing the doctor's information.</returns>
        [HttpGet("GetDoctor")]
        public async Task<IActionResult> GetDoctor([FromQuery] int id)
        {
            return Ok(await doctorService.GetDoctor(id));
        }

        /// <summary>
        /// Edits a doctor's information.
        /// </summary>
        /// <param name="model">The model containing the updated doctor's information.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpGet("Edit")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> Edit([FromQuery] DoctorDetailsModel model)
        {
            await doctorService.Edit(model, User.IsInRole("Director") ? User.FindFirstValue(ClaimTypes.NameIdentifier) : string.Empty);

            return Ok();
        }

        /// <summary>
        /// Retrieves a doctor by user ID.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <returns>An IActionResult containing the doctor's information.</returns>
        [HttpGet("GetDoctorByUserId")]
        public async Task<IActionResult> GetDoctorByUserId(string userId)
        {
            return Ok(await doctorService.GetDoctorByUserId(userId));
        }

        /// <summary>
        /// Retrieves all doctors associated with the currently logged-in director.
        /// </summary>
        /// <returns>An IActionResult containing the list of doctors.</returns>
        [HttpGet("AllByDirector")]
        [Authorize(Roles = "Director")]
        public async Task<IActionResult> AllByDirector()
        {
            var result = await doctorService.GetAllDoctorsByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(new { Doctors = result });
        }

        /// <summary>
        /// Retrieves the hospital ID associated with the currently logged-in director.
        /// </summary>
        /// <returns>An IActionResult containing the hospital ID.</returns>
        [HttpGet("HospitalIdByDirector")]
        [Authorize(Roles = "Director")]
        public async Task<IActionResult> HospitalIdByDirector()
        {
            var result = await doctorService.HospitalIdByDirector(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(new { HospitalId = result });
        }

        /// <summary>
        /// Retrieves the doctor ID for the currently logged-in user.
        /// </summary>
        /// <returns>An IActionResult containing the doctor ID.</returns>
        [HttpGet("GetDoctorId")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorId()
        {
            var result = await doctorService.GetDoctorByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(new { Id = result.Id });
        }

        /// <summary>
        /// Adds a rating for a specific doctor.
        /// </summary>
        /// <param name="rating">The rating value to add.</param>
        /// <param name="comment">The comment associated with the rating.</param>
        /// <param name="doctorId">The ID of the doctor being rated.</param>
        /// <param name="appointmentId">The ID of the appointment related to the rating.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpGet("AddRating")]
        [Authorize]
        public async Task<IActionResult> AddRating([FromQuery] float rating, string comment, int doctorId, int appointmentId)
        {
            var result = await doctorService.AddRating(rating, comment, doctorId, appointmentId, User.FindFirstValue(ClaimTypes.NameIdentifier));

            return result ? Ok(true) : BadRequest();
        }

        /// <summary>
        /// Checks if a specific appointment has a rating.
        /// </summary>
        /// <param name="appointmentId">The ID of the appointment to check.</param>
        /// <returns>An IActionResult indicating whether the appointment has a rating.</returns>
        [HttpGet("HasRating")]
        public async Task<IActionResult> HasRating([FromQuery] int appointmentId)
        {
            return Ok(await doctorService.AppointmentHasRating(appointmentId));
        }

        /// <summary>
        /// Retrieves the top doctors for a specific specialization.
        /// </summary>
        /// <param name="specialization">The specialization to search for.</param>
        /// <param name="top">The number of top doctors to retrieve.</param>
        /// <returns>An IActionResult containing the list of top doctors.</returns>
        [HttpGet("GetTopDoctorsWithSpecialization")]
        public async Task<IActionResult> GetTopDoctorsWithSpecialization([FromQuery] string specialization, int top)
        {
            return Ok(await doctorService.GetTopDoctorsWithSpecialization(specialization, top));
        }

        /// <summary>
        /// Retrieves the ratings for a specific doctor.
        /// </summary>
        /// <param name="doctorId">The ID of the doctor to get ratings for.</param>
        /// <returns>An IActionResult containing the doctor's ratings.</returns>
        [HttpGet("GetDoctorRatings")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> GetDoctorRatings([FromQuery] int doctorId)
        {
            return Ok(new { Ratings = await doctorService.GetDoctorRatings(doctorId) });
        }
    }
}
