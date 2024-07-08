using HealthSystemApi.Models.Service;
using HealthSystemApi.Services.ServiceService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystemApi.Controllers
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

        [HttpGet("Add")]
        public async Task<IActionResult> Add([FromQuery] ServiceAddModel model)
        {
            var result = await serviceService.AddAsync(model);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet("AllById")]
        public async Task<IActionResult> AllById([FromQuery] int id)
        {
            return Ok(await serviceService.AllByIdAsync(id));
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

            return Ok();
        }
    }
}
