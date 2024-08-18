using HealthSystem.Identity.Models;

namespace HealthSystem.Identity.Services.IdentityService
{
    public interface IIdentityService
    {
        string GetUserId();
        Task<bool> RemoveUser(string userId);
        Task<bool> IsAdministrator(string token);
        Task<bool> IsDoctor(string token);
        Task<bool> IsDirector(string token);
    }
}
