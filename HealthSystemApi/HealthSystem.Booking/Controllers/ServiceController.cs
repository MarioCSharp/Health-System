using HealthSystem.Booking.Models;
using HealthSystem.Booking.Services.ServiceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystem.Booking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private IServiceService serviceService;

        /// <summary>
        /// Constructor for ServiceController.
        /// </summary>
        /// <param name="serviceService">The service for managing services.</param>
        public ServiceController(IServiceService serviceService)
        {
            this.serviceService = serviceService;
        }

        /// <summary>
        /// Adds a new service.
        /// </summary>
        /// <param name="model">The model containing service details to add.</param>
        [HttpPost("Add")]
        [Authorize(Roles = "Administrator,Director,Doctor")]
        public async Task<IActionResult> Add([FromForm] ServiceAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await serviceService.AddAsync(model);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(new { Success = result });
        }

        /// <summary>
        /// Retrieves all services by ID.
        /// </summary>
        /// <param name="id">The service ID.</param>
        [HttpGet("AllById")]
        public async Task<IActionResult> AllById([FromQuery] int id)
        {
            var res = await serviceService.AllByIdAsync(id);

            return Ok(new { FullName = res.Item1, Services = res.Item2 });
        }

        /// <summary>
        /// Retrieves all services for the logged-in doctor.
        /// </summary>
        [HttpGet("GetDoctorServices")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorServices()
        {
            var res = await serviceService.AllByUserIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(new { Services = res });
        }

        /// <summary>
        /// Retrieves service details by ID.
        /// </summary>
        /// <param name="id">The service ID.</param>
        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            return Ok(await serviceService.DetailsAsync(id));
        }

        /// <summary>
        /// Books a service.
        /// </summary>
        /// <param name="model">The booking model.</param>
        [HttpGet("Book")]
        public async Task<IActionResult> Book([FromQuery] BookingModel model)
        {
            var isAvailable = await serviceService.BookAsync(model);

            if (!isAvailable)
            {
                return BadRequest();
            }

            return Ok(true);
        }

        /// <summary>
        /// Retrieves available hours for a specific service on a specific date.
        /// </summary>
        /// <param name="date">The date in yyyy-MM-dd format.</param>
        /// <param name="serviceId">The service ID.</param>
        [HttpGet("AvailableHours")]
        public async Task<IActionResult> AvailableHours([FromQuery] string date, [FromQuery] int serviceId)
        {
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                return BadRequest("Invalid date format. Please use yyyy-MM-dd.");
            }

            var doctorBookings = await serviceService.AvailableHoursAsync(parsedDate, serviceId);

            return Ok(doctorBookings);
        }

        /// <summary>
        /// Retrieves all services by user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        [HttpGet("AllByUser")]
        public async Task<IActionResult> AllByUser([FromQuery] string userId)
        {
            return Ok(await serviceService.AllByUserAsync(userId));
        }

        /// <summary>
        /// Deletes a service by ID.
        /// </summary>
        /// <param name="id">The service ID.</param>
        [HttpGet("Remove")]
        [Authorize(Roles = "Administrator,Director,Doctor")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await serviceService.Delete(id);

            return Ok(new { Success = result });
        }

        /// <summary>
        /// Retrieves service details for editing by ID.
        /// </summary>
        /// <param name="id">The service ID.</param>
        [HttpGet("Edit")]
        [Authorize(Roles = "Administrator,Director,Doctor")]
        public async Task<IActionResult> Edit([FromQuery] int id)
        {
            var result = await serviceService.EditGET(id);

            return Ok(new { ServiceName = result.Item1, ServicePrice = result.Item2, ServiceDescription = result.Item3, ServiceLocation = result.Item4 });
        }

        /// <summary>
        /// Updates a service.
        /// </summary>
        /// <param name="model">The service edit model.</param>
        [HttpPost("Edit")]
        [Authorize(Roles = "Administrator,Director,Doctor")]
        public async Task<IActionResult> Edit([FromForm] ServiceEditModel model)
        {
            var result = await serviceService.EditPOST(model);

            return Ok(new { Success = result });
        }

        /// <summary>
        /// Deletes all services by a specific doctor ID.
        /// </summary>
        /// <param name="doctorId">The doctor ID.</param>
        [HttpGet("DeleteAllByDoctorId")]
        [Authorize(Roles = "Administrator,Director")]
        public async Task<IActionResult> DeleteAllByDoctorId([FromQuery] int doctorId)
        {
            await serviceService.DeleteAllByDoctorId(doctorId);

            return Ok();
        }
    }
}
