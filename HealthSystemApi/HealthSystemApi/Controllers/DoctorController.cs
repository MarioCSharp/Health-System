using HealthSystemApi.Data;
using HealthSystemApi.Models.Doctor;
using HealthSystemApi.Services.DoctorService;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("All")]
        public async Task<IActionResult> All([FromQuery] int id)
        {
            var result = await doctorService.GetAllAsync(id);

            return Ok(result);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            return Ok(await doctorService.GetDetailsAsync(id));
        }

        [HttpGet("GetDoctor")]
        public async Task<IActionResult> GetDoctor([FromQuery] int id)
        {
            return Ok(await doctorService.GetDoctor(id));
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit([FromQuery] DoctorDetailsModel model)
        {
            await doctorService.Edit(model);

            return Ok();
        }
    }
}
