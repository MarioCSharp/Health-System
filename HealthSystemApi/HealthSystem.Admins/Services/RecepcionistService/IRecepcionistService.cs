using HealthSystem.Admins.Models;

namespace HealthSystem.Admins.Services.RecepcionistService
{
    public interface IRecepcionistService
    {
        Task<bool> AddAsync(string userId, int hospitalId, string name, string token);
        Task<int> GetHospitalIdAsync(string userId);
        Task<List<RecepcionistDisplayModel>> GetMyRecepcionists(string userId);
        Task Delete(int id, string token);
    }
}
