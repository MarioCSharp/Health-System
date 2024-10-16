﻿using HealthSystem.Problems.Models;

namespace HealthSystem.Problems.Services.ProblemService
{
    public interface IProblemService
    {
        Task<bool> AddAsync(ProblemAddModel problemAddModel, List<int> symptoms, string? userId);
        Task AddSymptomsAsync();
        Task<ProblemDetailsModel> DetailsAsync(int id);
        Task<bool> EditAsync(ProblemEditModel healthIssueEditModel, List<int> symptoms);
        Task<bool> RemoveAsync(int id);
        Task<List<ProblemDisplayModel>> UserProblemsAsync(string? userId);
        Task<List<SymptomCategoryDisplayModel>> LoadCategoriesForMAUI();
        Task<List<SymptomSubCategoryDisplayModel>> LoadSubCategoriesForMAUI();
    }
}
