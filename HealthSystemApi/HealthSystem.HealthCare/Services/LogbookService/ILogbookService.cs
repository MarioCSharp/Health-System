using HealthSystem.HealthCare.Models;

namespace HealthSystem.HealthCare.Services.LogbookService
{
    public interface ILogbookService
    {
        Task<bool> AddAsync(LogAddModel model);
        Task<List<LogDisplayModel>> AllByUserAsync(string userId);
        Task<LogAddModel> GetEditAsync(int id);
        Task<bool> EditAsync(LogAddModel model);
        Task<bool> DeleteAsync(int id);
    }
}
