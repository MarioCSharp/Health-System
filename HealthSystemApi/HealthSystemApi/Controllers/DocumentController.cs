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

        [HttpGet("Add")]
        public async Task<IActionResult> Add([FromQuery] DocumentAddModel model, [FromQuery] IFormFile File, [FromQuery] string userId)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
            }

            var result = await documentService.AddAsync(model, userId, File);

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
