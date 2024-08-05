using HealthProject.Models;

namespace HealthProject.Services.LogbookService
{
    public interface ILogbookService
    {
        Task<List<LogDisplayModel>> GetByUser(string userId);
        Task<LogAddModel> GetEditAsync(int id);
        Task<bool> EditAsync(LogAddModel model);
        Task<bool> AddAsync(LogAddModel model);
        Task<bool> DeleteAsync(int id);
    }
}
