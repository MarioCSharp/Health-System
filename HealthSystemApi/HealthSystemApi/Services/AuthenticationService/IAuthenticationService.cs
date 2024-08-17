namespace HealthSystemApi.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        string GetUserId();
        Task<bool> RemoveUser(string userId);
        Task<bool> IsAdministrator(string token);
        Task<bool> IsDoctor(string token);
        Task<bool> IsDirector(string token);
    }
}
