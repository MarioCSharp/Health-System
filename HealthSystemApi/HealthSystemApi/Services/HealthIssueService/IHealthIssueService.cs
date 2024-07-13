using HealthSystemApi.Models.HealthIssue;

namespace HealthSystemApi.Services.HealthIssueService
{
    public interface IHealthIssueService
    {
        Task<bool> AddAsync(HealthIssueAddModel healthIssueAddModel, string userId);
        Task<bool> RemoveAsync(int id);
        Task<HealthIssueDetailsModel> DetailsAsync(int id);
        Task<bool> EditAsync(HealthIssueEditModel healthIssueEditModel);
        Task<List<HealthIssueDisplayModel>> UserIssuesAsync(string userId);
    }
}
