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

        /// <summary>
        /// Initializes a new instance of the <see cref="HospitalController"/> class.
        /// </summary>
        /// <param name="hospitalService">The service for handling hospital-related operations.</param>
        public HospitalController(IHospitalService hospitalService)
        {
            this.hospitalService = hospitalService;
        }

        /// <summary>
        /// Adds a new hospital.
        /// </summary>
        /// <param name="model">The model containing information about the hospital to add.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
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

        /// <summary>
        /// Removes a hospital by ID.
        /// </summary>
        /// <param name="id">The ID of the hospital to remove.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
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

        /// <summary>
        /// Retrieves all hospitals.
        /// </summary>
        /// <returns>An IActionResult containing the list of hospitals.</returns>
        [HttpGet("All")]
        public async Task<IActionResult> All()
        {
            var hospitals = await hospitalService.AllAsync();

            return Ok(new { Hospitals = hospitals });
        }

        /// <summary>
        /// Retrieves details of a specific hospital.
        /// </summary>
        /// <param name="id">The ID of the hospital to get details for.</param>
        /// <returns>An IActionResult containing the hospital's details.</returns>
        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            var result = await hospitalService.HospitalDetails(id);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves doctors associated with a specific hospital.
        /// </summary>
        /// <param name="id">The ID of the hospital to get doctors from.</param>
        /// <returns>An IActionResult containing the list of doctors.</returns>
        [HttpGet("GetDoctors")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> GetDoctors([FromQuery] int id)
        {
            var doctors = await hospitalService.GetDoctorsAsync(id, User.IsInRole("Director") ? User.FindFirstValue(ClaimTypes.NameIdentifier) : string.Empty);

            return Ok(new { Doctors = doctors });
        }

        /// <summary>
        /// Retrieves information about a specific hospital.
        /// </summary>
        /// <param name="hospitalId">The ID of the hospital to retrieve.</param>
        /// <returns>An IActionResult containing the hospital's information.</returns>
        [HttpGet("GetHospital")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> GetHospital([FromQuery] int hospitalId)
        {
            var hospital = await hospitalService.GetHospital(hospitalId, User.IsInRole("Director") ? User.FindFirstValue(ClaimTypes.NameIdentifier) : string.Empty);

            return Ok(new { ContactNumber = hospital.ContactNumber, Location = hospital.Location, Name = hospital.HospitalName, UserId = hospital.UserId });
        }

        /// <summary>
        /// Edits information about a hospital.
        /// </summary>
        /// <param name="model">The model containing updated information about the hospital.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("Edit")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> Edit([FromForm] HospitalEditModel model)
        {
            var result = await hospitalService.EditAsync(model, User.IsInRole("Director") ? User.FindFirstValue(ClaimTypes.NameIdentifier) : string.Empty);

            return Ok(new { Success = result });
        }

        /// <summary>
        /// Retrieves the hospital associated with the currently logged-in director using a token.
        /// </summary>
        /// <param name="token">The token used to identify the hospital.</param>
        /// <returns>An IActionResult containing the hospital's information.</returns>
        [HttpGet("GetDirectorHospital")]
        [Authorize(Roles = "Director")]
        public async Task<IActionResult> GetDirectorHospital([FromQuery] string token)
        {
            var hospital = await hospitalService.GetHospitalByToken(token);

            return Ok(new { Hospital = hospital });
        }

        /// <summary>
        /// Retrieves the hospital ID associated with the currently logged-in director.
        /// </summary>
        /// <returns>An IActionResult containing the hospital ID.</returns>
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
