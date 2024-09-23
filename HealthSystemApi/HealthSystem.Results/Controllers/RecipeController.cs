using HealthSystem.Results.Models;
using HealthSystem.Results.Services.RecipeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.Results.Controllers
{
    /// <summary>
    /// API controller for managing recipes (prescriptions).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private IRecipeService recipeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeController"/> class.
        /// </summary>
        /// <param name="recipeService">The service for managing recipes.</param>
        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        /// <summary>
        /// Adds a new recipe (prescription) by a doctor.
        /// </summary>
        /// <param name="model">The model containing recipe details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating success or failure.</returns>
        [HttpPost("AddRecipe")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AddRecipe([FromForm] IssueRecipeModel model)
        {
            if (model.FormFile == null || model == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await recipeService.AddRecipeService(model, model.FormFile);

            return result ? Ok() : BadRequest();
        }

        /// <summary>
        /// Retrieves all recipes associated with a specific EGN (national identification number).
        /// </summary>
        /// <param name="EGN">The EGN (national identification number) of the patient.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of recipes.</returns>
        [HttpGet("GetRecipies")]
        [Authorize]
        public async Task<IActionResult> GetRecipies([FromQuery] string EGN)
        {
            var result = await recipeService.GetRecipiesAsync(EGN);

            return Ok(new { Recipes = result });
        }

        /// <summary>
        /// Retrieves the latest recipe associated with a specific EGN.
        /// </summary>
        /// <param name="EGN">The EGN (national identification number) of the patient.</param>
        /// <returns>A file containing the latest recipe.</returns>
        [HttpGet("GeLastRecipe")]
        [Authorize]
        public async Task<IActionResult> GeLasttRecipe([FromQuery] string EGN)
        {
            var file = await recipeService.GetLastRecipe(EGN);

            return File(file.OpenReadStream(), file.ContentType, file.FileName);
        }

        /// <summary>
        /// Downloads a specific recipe based on the recipe ID.
        /// </summary>
        /// <param name="recipeId">The ID of the recipe to download.</param>
        /// <returns>A file containing the recipe.</returns>
        [HttpGet("DownloadRecipe")]
        [Authorize]
        public async Task<IActionResult> DownloadRecipe([FromQuery] int recipeId)
        {
            var file = await recipeService.GetRecipeFileAsync(recipeId);

            return File(file.OpenReadStream(), file.ContentType, file.FileName);
        }
    }
}
