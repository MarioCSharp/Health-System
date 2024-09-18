using HealthSystem.Documents.Models;
using HealthSystem.Documents.Services.DocumentService;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.Documents.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private IDocumentService documentService;

        /// <summary>
        /// Initializes a new instance of the DocumentController class.
        /// </summary>
        /// <param name="documentService">The document service used for managing documents.</param>
        public DocumentController(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        /// <summary>
        /// Adds a new document asynchronously.
        /// </summary>
        /// <param name="model">The document model containing metadata.</param>
        /// <param name="file">The file to be uploaded.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
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

        /// <summary>
        /// Removes a document asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the document to remove.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
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

        /// <summary>
        /// Edits a document asynchronously.
        /// </summary>
        /// <param name="model">The model containing updated document information.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
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

        /// <summary>
        /// Retrieves the details of a document asynchronously by its ID.
        /// </summary>
        /// <param name="id">The ID of the document to retrieve.</param>
        /// <returns>An IActionResult containing the document details.</returns>
        [HttpGet("Details")]
        public async Task<IActionResult> Details([FromQuery] int id)
        {
            return Ok(await documentService.DetailsAsync(id));
        }

        /// <summary>
        /// Retrieves all documents associated with a user asynchronously.
        /// </summary>
        /// <param name="userId">The user ID to retrieve documents for.</param>
        /// <returns>An IActionResult containing a list of documents associated with the user.</returns>
        [HttpGet("AllByUser")]
        public async Task<IActionResult> AllByUser([FromQuery] string userId)
        {
            return Ok(await documentService.AllByUser(userId));
        }
    }
}
