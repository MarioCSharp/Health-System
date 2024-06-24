using HealthProject.Models;

namespace HealthProject.Services
{
    public interface IAuthenticationService
    {
        Task Register(RegisterModel registerModel);
    }
}
