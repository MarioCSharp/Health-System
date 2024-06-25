using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Services.AuthenticationService;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        public ICommand EnterCommand { get; }
        private IAuthenticationService authenticationService;
        public MainPageViewModel(IAuthenticationService authenticationService)
        {
            EnterCommand = new AsyncRelayCommand(EnterAsync);
            this.authenticationService = authenticationService;
        }

        public async Task EnterAsync()
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (authToken.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}
