using HealthSystem.Results.Models;
using HealthSystem.Results.Services.RecipeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthSystem.Results.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private IRecipeService recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        [HttpPost("AddRecipe")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AddRecipe([FromForm] IssueRecipeModel model)
        {
            var result = await recipeService.AddRecipeService(model);

            return result ? Ok() : BadRequest();    
        }

        [HttpPost("GetRecipies")]
        [Authorize]
        public async Task<IActionResult> GetRecipies([FromQuery] string EGN)
        {
            var result = await recipeService.GetRecipiesAsync(EGN);

            return Ok(new { Recipes = result });
        }

        [HttpGet("DownloadRecipe")]
        [Authorize]
        public async Task<IActionResult> DownloadRecipe([FromQuery] int recipeId)
        {
            var file = await recipeService.GetRecipeFileAsync(recipeId);

            return File(file.OpenReadStream(), file.ContentType, file.FileName);
        }
    }
}
