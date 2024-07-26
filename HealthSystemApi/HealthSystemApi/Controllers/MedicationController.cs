using HealthSystemApi.Models.Medication;
using HealthSystemApi.Services.MedicationService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicationController : ControllerBase
    {
        private IMedicationService medicationService;

        public MedicationController(IMedicationService medicationService)
        {
            this.medicationService = medicationService;
        }

        [HttpGet("Add")]
        public async Task<IActionResult> Add([FromQuery] MedicationAddModel medicationModel, [FromQuery] MedicationScheduleAddModel scheduleModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await medicationService.AddAsync(medicationModel, scheduleModel);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(true);
        }

        [HttpGet("Remove")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            return Ok(await medicationService.DeleteAsync(id));
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            return Ok(await medicationService.DetailsAsync(id));
        }
    }
}
