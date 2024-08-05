using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.LogbookService;
using HealthProject.Views;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class LogbookAddViewModel : ObservableObject
    {
        [ObservableProperty]
        private LogAddModel log;

        private ILogbookService logbookService;
        private IAuthenticationService authenticationService;

        public LogbookAddViewModel(ILogbookService logbookService,
                                   IAuthenticationService authenticationService)
        {
            Log = new LogAddModel();

            this.authenticationService = authenticationService;
            this.logbookService = logbookService;

            AddCommand = new AsyncRelayCommand(AddAsync);
        }

        public ICommand AddCommand { get; }

        public async Task AddAsync()
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (!authToken.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            var result = await logbookService.AddAsync(Log);

            if (result) 
            {
                await Shell.Current.GoToAsync($"//{nameof(LogbookViewPage)}");
            }
        }
    }
}
