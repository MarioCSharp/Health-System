using HealthSystemApi.Models.Service;
using HealthSystemApi.Services.AuthenticationService;
using HealthSystemApi.Services.ServiceService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace HealthSystemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private IServiceService serviceService;
        private IAuthenticationService authenticationService;

        public ServiceController(IServiceService serviceService,
                                 IAuthenticationService authenticationService)
        {
            this.serviceService = serviceService;
            this.authenticationService = authenticationService;
        }

        [HttpGet("Add")]
        public async Task<IActionResult> Add([FromQuery] ServiceAddModel model)
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
        public async Task<IActionResult> Delete([FromQuery] int id, string token)
        {
            var isAdmin = await authenticationService.IsAdministrator(token);

            if (!isAdmin)
            {
                return BadRequest();
            }

            var result = await serviceService.Delete(id);

            return Ok(new { Success = result });
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit([FromQuery] int id, string token)
        {
            var isAdmin = await authenticationService.IsAdministrator(token);

            if (!isAdmin)
            {
                return BadRequest();
            }

            var result = await serviceService.EditGET(id);

            return Ok(new { ServiceName = result.Item1, ServicePrice = result.Item2, ServiceDescription = result.Item3, ServiceLocation = result.Item4 });
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromForm] ServiceEditModel model)
        {
            var isAdmin = await authenticationService.IsAdministrator(model.Token);

            if (!isAdmin)
            {
                return BadRequest();
            }

            var result = await serviceService.EditPOST(model);

            return Ok(new { Success = result });
        }
    }
}
