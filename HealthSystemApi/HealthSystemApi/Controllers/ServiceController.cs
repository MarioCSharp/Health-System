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
    }
}
