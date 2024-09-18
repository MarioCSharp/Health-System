using HealthSystem.Results.Data;
using HealthSystem.Results.Data.Models;
using HealthSystem.Results.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthSystem.Results.Services.RecipeService
{
    public class RecipeService : IRecipeService
    {
        private ResultsDbContext context;

        public RecipeService(ResultsDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddRecipeService(IssueRecipeModel model, IFormFile File)
        {
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await File.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var recipe = new IssuedRecipe()
            {
                DoctorName = model.DoctorName,
                EGN = model.PatientEGN,
                File = fileBytes, 
                PatientName = model.PatientName
            };

            await context.IssuedRecipes.AddAsync(recipe);
            await context.SaveChangesAsync();

            return await context.IssuedRecipes.ContainsAsync(recipe);
        }

        public async Task<IFormFile> GetLastRecipe(string EGN)
        {
            var recipe = await context.IssuedRecipes
                             .Where(r => r.EGN == EGN)
                             .OrderByDescending(r => r.Id) 
                             .LastOrDefaultAsync();

            if (recipe == null)
            {
                return null;
            }

            var stream = new MemoryStream(recipe.File);

            var formFile = new FormFile(stream, 0, recipe.File.Length, "file", "Рецепта.txt") 
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain" 
            };

            return formFile;
        }


        public async Task<IFormFile> GetRecipeFileAsync(int recipeId)
        {
            var recipe = await context.IssuedRecipes.FindAsync(recipeId);

            if (recipe == null) 
            {
                return null;
            }

            var stream = new MemoryStream(recipe.File);

            var formFile = new FormFile(stream, 0, recipe.File.Length, "file", "Рецепта")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            return formFile;
        }

        public async Task<List<RecipeDisplayModel>> GetRecipiesAsync(string egn)
        {
            return await context.IssuedRecipes
                .Where(x => x.EGN == egn)
                .Select(x => new RecipeDisplayModel()
                {
                    Id = x.Id,
                    DoctorName = x.DoctorName,
                    PatientName = x.PatientName
                }).ToListAsync();
        }
    }
}
