using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class RegisterPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private RegisterModel registerModel;

        public ICommand RegisterCommand { get; }
        public ICommand LoginCommand { get; }
        private IAuthenticationService authenticationService;
        public RegisterPageViewModel(IAuthenticationService authenticationService)
        {
            RegisterCommand = new AsyncRelayCommand(RegisterAsync);
            LoginCommand = new AsyncRelayCommand(LoginAsync);
            this.authenticationService = authenticationService;
            this.registerModel = new RegisterModel();
        }

        public async Task RegisterAsync()
        {
            await authenticationService.Register(registerModel);

            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }

        public async Task LoginAsync()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
