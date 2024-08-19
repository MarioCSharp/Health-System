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

        public ServiceController(IServiceService serviceService)
        {
            this.serviceService = serviceService;
        }

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

        [HttpGet("AllById")]
        public async Task<IActionResult> AllById([FromQuery] int id)
        {
            var res = await serviceService.AllByIdAsync(id);

            return Ok(new { FullName = res.Item1, Services = res.Item2 });
        }

        [HttpGet("GetDoctorServices")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorServices()
        {
            var res = await serviceService.AllByUserIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return Ok(new { Services = res });
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            return Ok(await serviceService.DetailsAsync(id));
        }

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

        [HttpGet("AllByUser")]
        public async Task<IActionResult> AllByUser([FromQuery] string userId)
        {
            return Ok(await serviceService.AllByUserAsync(userId));
        }

        [HttpGet("Remove")]
        [Authorize(Roles = "Administrator,Director,Doctor")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await serviceService.Delete(id);

            return Ok(new { Success = result });
        }

        [HttpGet("Edit")]
        [Authorize(Roles = "Administrator,Director,Doctor")]
        public async Task<IActionResult> Edit([FromQuery] int id)
        {
            var result = await serviceService.EditGET(id);

            return Ok(new { ServiceName = result.Item1, ServicePrice = result.Item2, ServiceDescription = result.Item3, ServiceLocation = result.Item4 });
        }

        [HttpPost("Edit")]
        [Authorize(Roles = "Administrator,Director,Doctor")]
        public async Task<IActionResult> Edit([FromForm] ServiceEditModel model)
        {
            var result = await serviceService.EditPOST(model);

            return Ok(new { Success = result });
        }

        [HttpGet("DeleteAllByDoctorId")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteAllByDoctorId([FromQuery] int doctorId)
        {
            await serviceService.DeleteAllByDoctorId(doctorId);

            return Ok();
        }
    }
}
