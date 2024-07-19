using HealthProject.Models;

namespace HealthProject.Services.ProblemService
{
    public interface IProblemService
    {
        Task<List<ProblemDisplayModel>> GetUserProblems(string? userId);
        Task<bool> AddAsync(ProblemAddModel model, List<int> symptomsIds);
        Task DeleteAsync(int id);
        Task<ProblemDetailsModel> DetailsAsync(int id);
        Task<List<SymptomCategoryDisplayModel>> GetSymptomsCategories();
        Task<List<SymptomSubCategoryDisplayModel>> GetSymptomsSubCategories();
    }
}
