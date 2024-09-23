using HealthSystem.Results.Data;
using HealthSystem.Results.Data.Models;
using HealthSystem.Results.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthSystem.Results.Services.RecipeService
{
    /// <summary>
    /// Service class responsible for handling operations related to medical recipes.
    /// </summary>
    public class RecipeService : IRecipeService
    {
        private readonly ResultsDbContext context;

        /// <summary>
        /// Constructor to initialize the RecipeService with the database context.
        /// </summary>
        /// <param name="context">The database context for accessing recipe data.</param>
        public RecipeService(ResultsDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds a new recipe to the database.
        /// </summary>
        /// <param name="model">The data model containing recipe details.</param>
        /// <param name="File">The file associated with the recipe.</param>
        /// <returns>Returns true if the recipe was successfully added, otherwise false.</returns>
        public async Task<bool> AddRecipeService(IssueRecipeModel model, IFormFile File)
        {
            // Convert the file to a byte array
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await File.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            // Create a new IssuedRecipe instance
            var recipe = new IssuedRecipe()
            {
                DoctorName = model.DoctorName,
                EGN = model.PatientEGN,
                File = fileBytes,
                PatientName = model.PatientName
            };

            // Add the recipe to the database and save changes
            await context.IssuedRecipes.AddAsync(recipe);
            await context.SaveChangesAsync();

            return await context.IssuedRecipes.ContainsAsync(recipe);
        }

        /// <summary>
        /// Retrieves the last added recipe for a patient by their EGN (personal identification number).
        /// </summary>
        /// <param name="EGN">The EGN of the patient.</param>
        /// <returns>Returns the last issued recipe as an IFormFile, or null if no recipe exists.</returns>
        public async Task<IFormFile> GetLastRecipe(string EGN)
        {
            // Fetch the latest recipe based on the patient's EGN
            var recipe = await context.IssuedRecipes
                             .Where(r => r.EGN == EGN)
                             .OrderByDescending(r => r.Id) // Get the most recent recipe
                             .FirstOrDefaultAsync();

            if (recipe == null)
            {
                return null;
            }

            // Create a MemoryStream from the file bytes and return it as an IFormFile
            var stream = new MemoryStream(recipe.File);

            var formFile = new FormFile(stream, 0, recipe.File.Length, "file", "Рецепта.txt")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain" // Assuming the file is a plain text file
            };

            return formFile;
        }

        /// <summary>
        /// Retrieves a specific recipe file based on the recipe's ID.
        /// </summary>
        /// <param name="recipeId">The ID of the recipe.</param>
        /// <returns>Returns the recipe file as an IFormFile, or null if not found.</returns>
        public async Task<IFormFile> GetRecipeFileAsync(int recipeId)
        {
            var recipe = await context.IssuedRecipes.FindAsync(recipeId);

            if (recipe == null)
            {
                return null;
            }

            // Create a MemoryStream from the file bytes and return it as an IFormFile
            var stream = new MemoryStream(recipe.File);

            var formFile = new FormFile(stream, 0, recipe.File.Length, "file", "Рецепта.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf" // Assuming the file is a PDF
            };

            return formFile;
        }

        /// <summary>
        /// Retrieves all recipes for a patient based on their EGN.
        /// </summary>
        /// <param name="egn">The EGN of the patient.</param>
        /// <returns>Returns a list of recipes as display models.</returns>
        public async Task<List<RecipeDisplayModel>> GetRecipiesAsync(string egn)
        {
            // Fetch the recipes associated with the given EGN
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
