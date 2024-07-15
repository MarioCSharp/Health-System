using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using System.Windows.Input;
using HealthProject.Views;
namespace HealthProject.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private LoginModel loginModel;

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        private IAuthenticationService authenticationService;
        public LoginPageViewModel(IAuthenticationService authenticationService)
        {
            LoginCommand = new AsyncRelayCommand(LoginAsync);
            RegisterCommand = new AsyncRelayCommand(RegisterAsync);
            this.authenticationService = authenticationService;
            this.loginModel = new LoginModel();
        }

        public async Task LoginAsync()
        {
            await authenticationService.Login(loginModel);

            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }

        public async Task RegisterAsync()
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }
    }
}
