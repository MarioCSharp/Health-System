using HealthSystem.Documents.Models;

namespace HealthSystem.Documents.Services.ReminderService
{
    public interface IReminderService
    {
        Task<bool> AddAsync(ReminderAddModel model, string userId);
        Task<bool> RemoveAsync(int id, string userId);
        Task<List<ReminderDetailsModel>> AllByUserAsync(string userId);
    }
}
