using HealthSystemApi.Data;
using HealthSystemApi.Data.Models;
using HealthSystemApi.Models.Doctor;
using HealthSystemApi.Services.DoctorService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private ApplicationDbContext context;
        private IDoctorService doctorService;

        public DoctorController(ApplicationDbContext context,
                                IDoctorService doctorService)
        {
            this.context = context;
            this.doctorService = doctorService;
        }

        [HttpGet("Add")]
        public async Task<IActionResult> Add([FromQuery] DoctorAddModel model)
        {
            var result = await doctorService.AddAsync(model);

            return result ? Ok() : BadRequest();
        }

        [HttpGet("Remove")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            var result = await doctorService.RemoveAsync(id);

            return result ? Ok() : BadRequest();
        }
    }
}
