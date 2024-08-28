using HealthSystem.Results.Models;

namespace HealthSystem.Results.Services.LaboratoryResultService
{
    public interface ILaboratoryResultService
    {
        Task<(bool, string, string)> IssueResultAsync(IssueResultModel model, string userId);
        Task<byte[]> GetFileAsync(string id, string pass);
        Task<bool> AddFileAsync(int appointmentId, IFormFile file);
        Task<List<LaboratoryResultDisplayModel>> GetResults(string doctorUserId);
    }
}
