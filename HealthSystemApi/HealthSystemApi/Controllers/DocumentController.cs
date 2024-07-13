using HealthSystemApi.Models.Document;
using HealthSystemApi.Services.DocumentService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystemApi.Controllers
{
    [ApiController]
    [Route("api/[contoller]")]
    public class DocumentController : ControllerBase
    {
        private IDocumentService documentService;

        public DocumentController(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        [HttpGet("Add")]
        public async Task<IActionResult> Add([FromQuery] DocumentAddModel model, [FromQuery] IFormFile File)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
            }

            var result = await documentService.AddAsync(model, User.FindFirstValue(ClaimTypes.NameIdentifier), File);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet("Remove")]
        public async Task<IActionResult> Remove([FromQuery] int id)
        {
            var result = await documentService.RemoveAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit([FromQuery] DocumentEditModel model)
        {
            var result = await documentService.EditAsync(model);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            return Ok(await documentService.DetailsAsync(id));
        }

        [HttpGet("AllByUser")]
        public async Task<IActionResult> Details()
        {
            return Ok(await documentService.AllByUser(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }
    }
}
