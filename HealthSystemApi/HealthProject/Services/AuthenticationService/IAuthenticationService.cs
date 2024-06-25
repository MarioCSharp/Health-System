using HealthProject.Models;

namespace HealthProject.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task Register(RegisterModel registerModel);
        Task Login(LoginModel registerModel);
        Task<AuthenticationModel> IsAuthenticated();
    }
}
