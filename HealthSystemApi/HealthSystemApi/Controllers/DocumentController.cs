using HealthSystemApi.Models.Document;
using HealthSystemApi.Services.DocumentService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthSystemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private IDocumentService documentService;

        public DocumentController(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] DocumentAddModel model, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model state");
            }

            var result = await documentService.AddAsync(model, model.UserId, file);

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
        public async Task<IActionResult> AllByUser([FromQuery] string userId)
        {
            return Ok(await documentService.AllByUser(userId));
        }
    }
}
