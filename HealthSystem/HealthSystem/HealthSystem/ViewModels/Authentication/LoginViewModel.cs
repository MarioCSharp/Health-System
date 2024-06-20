using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace HealthSystem.ViewModels.Authentication
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string password;

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(LoginAsync);
        }

        private async Task LoginAsync()
        {
            Application.Current.MainPage = new MainPage();
            System.Diagnostics.Debug.WriteLine("Login command executed");
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
    }
}
