using HealthProject.Models;

namespace HealthProject.Services.ReminderService
{
    public interface IReminderService
    {
        Task<bool> AddAsync(ReminderAddModel model);
        Task<bool> DeleteAsync(int id);
        Task<List<ReminderDisplayModel>> AllByUser();
    }
}
