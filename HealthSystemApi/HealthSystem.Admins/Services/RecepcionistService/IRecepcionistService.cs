namespace HealthSystem.Admins.Services.RecepcionistService
{
    public interface IRecepcionistService
    {
        Task<bool> AddAsync(string userId, int hospitalId, string name);
        Task<int> GetHospitalIdAsync(string userId);
    }
}
