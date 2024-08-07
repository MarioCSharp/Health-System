using HealthSystemApi.Models.Logbook;
using HealthSystemApi.Services.LogbookService;
using Microsoft.AspNetCore.Mvc;
namespace HealthSystemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogbookController : ControllerBase
    {
        private ILogbookService logbookService;

        public LogbookController(ILogbookService logbookService)
        {
            this.logbookService = logbookService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] LogAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = await logbookService.AddAsync(model);

            if (!result)
            {
                return Ok(false);
            }

            return Ok(result);
        }

        [HttpGet("Edit")]

        public async Task<IActionResult> Edit([FromQuery] int id)
        {
            return Ok(await logbookService.GetEditAsync(id));
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit([FromForm] LogAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await logbookService.EditAsync(model);

            if (!result) 
            { 
                return Ok(false);
            }

            return Ok(result);
        }

        [HttpGet("Remove")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            return Ok(await logbookService.DeleteAsync(id));
        }

        [HttpGet("AllByUser")]
        public async Task<IActionResult> AllByUser([FromQuery] string userId)
        {
            return Ok(await logbookService.AllByUserAsync(userId));
        }
    }
}
