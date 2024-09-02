using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.LogbookService;
using HealthProject.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class LogbookPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<LogDisplayModel> logbook;

        private ILogbookService logbookService;
        private IAuthenticationService authenticationService;

        public LogbookPageViewModel(ILogbookService logbookService,
                                    IAuthenticationService authenticationService)
        {
            this.logbookService = logbookService;
            this.authenticationService = authenticationService;

            LaodLog();

            RedirectToAdd = new AsyncRelayCommand(RedirectToAddAsync);
            RedirectToEdit = new AsyncRelayCommand<object>(RedirectToEditAsync);
            DeleteCommand = new AsyncRelayCommand<object>(DeleteAsync);
        }

        public ICommand RedirectToAdd { get; }
        public ICommand RedirectToEdit { get; }
        public ICommand DeleteCommand { get; }

        public async void LaodLog()
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (!authToken.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            var logs = await logbookService.GetByUser(authToken.UserId ?? string.Empty);

            Logbook = new ObservableCollection<LogDisplayModel>(logs);
        }

        public async Task RedirectToAddAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(LogbookAddPage)}");
        }

        public async Task RedirectToEditAsync(object parameter)
        {
            if (parameter is int id)
            {
                var log = await logbookService.GetEditAsync(id);

                var logJson = JsonConvert.SerializeObject(log);
                var encodedLogJson = Uri.EscapeDataString(logJson);

                await Shell.Current.GoToAsync($"{nameof(LogbookEditPage)}?logJson={encodedLogJson}");
            }
        }

        public async Task DeleteAsync(object parameter)
        {
            if (parameter is int id)
            {
                await logbookService.DeleteAsync(id);
                LaodLog();
                await Shell.Current.GoToAsync($"//{nameof(LogbookViewPage)}");
            }
        }
    }
}
