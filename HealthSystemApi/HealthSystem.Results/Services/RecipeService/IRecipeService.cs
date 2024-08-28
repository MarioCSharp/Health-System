using HealthSystem.Results.Models;

namespace HealthSystem.Results.Services.RecipeService
{
    public interface IRecipeService
    {
        Task<bool> AddRecipeService(IssueRecipeModel model);
        Task<List<RecipeDisplayModel>> GetRecipiesAsync(string egn);
        Task<IFormFile> GetRecipeFileAsync(int recipeId);
    }
}
