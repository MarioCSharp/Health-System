namespace HealthProject.Services.NavigationService
{
    public interface INavigationService
    {
        Task NavigateToAsync(string pageKey, object parameter = null);
    }
}
