using HealthProject.Models;

namespace HealthProject.Services.HealthIssueService
{
    public interface IHealthIssueService
    {
        Task<List<HealthIssueDisplayModel>> AllByUser(string userId);
        Task Remove(int id);
        Task<HealthIssueDetailsModel> DetailsAsync(int id);
        Task<bool> AddAsync(HealthIssueAddModel model, string userId);
    }
}
