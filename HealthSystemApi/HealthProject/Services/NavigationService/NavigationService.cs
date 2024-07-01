namespace HealthProject.Services.NavigationService
{
    public class NavigationService : INavigationService
    {
        public async Task NavigateToAsync(string pageKey, object parameter = null)
        {
            var pageType = Type.GetType(pageKey);
            if (pageType != null)
            {
                var page = (Page)Activator.CreateInstance(pageType, parameter);
                if (Application.Current.MainPage is NavigationPage navigationPage)
                {
                    await navigationPage.PushAsync(page);
                }
            }
        }
    }
}
