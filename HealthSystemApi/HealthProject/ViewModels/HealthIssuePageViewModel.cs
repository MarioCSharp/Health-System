using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.HealthIssueService;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HealthProject.Views;
namespace HealthProject.ViewModels
{
    public partial class HealthIssuePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<HealthIssueDisplayModel> healthIssues;

        public ICommand AddHealthIssueCommand { get; }
        public ICommand DeleteHealthIssueCommand { get; }
        public ICommand NavigateToHealthIssueDetailsCommand { get; }

        private IHealthIssueService healthIssueService;
        private IAuthenticationService authenticationService;

        public HealthIssuePageViewModel(IHealthIssueService healthIssueService,
                                        IAuthenticationService authenticationService)
        {
            this.healthIssueService = healthIssueService;
            this.authenticationService = authenticationService;
            AddHealthIssueCommand = new AsyncRelayCommand(RedirectToAddHealthIssueAsync);
            DeleteHealthIssueCommand = new AsyncRelayCommand<object>(DeleteHealthIssueAsync);
            NavigateToHealthIssueDetailsCommand = new AsyncRelayCommand<object>(NavigateToHealthIssueDetailsAsync);
            LoadHealthIssues();
        }

        public async Task RedirectToAddHealthIssueAsync()
        {
            var auth = await authenticationService.IsAuthenticated();

            if (!auth.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            var userId = auth.UserId;

            await Shell.Current.GoToAsync($"{nameof(HealthIssueAddPage)}?userId={userId}");
        }

        public async Task DeleteHealthIssueAsync(object parameter)
        {
            if (parameter is int id)
            {
                await healthIssueService.Remove(id);
                LoadHealthIssues();
            }
        }

        public async Task NavigateToHealthIssueDetailsAsync(object parameter)
        {
            if (parameter is int id)
            {
                var hI = await healthIssueService.DetailsAsync(id);

                var hIJson = JsonConvert.SerializeObject(hI);
                var encodedHIJson = Uri.EscapeDataString(hIJson);

                await Shell.Current.GoToAsync($"HealtIssueDetailsPage?healthIssueJson={encodedHIJson}");
            }
        }

        public async void LoadHealthIssues()
        {
            var auth = await authenticationService.IsAuthenticated();

            if (!auth.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }

            var healthIssues = await healthIssueService.AllByUser(auth.UserId);

            HealthIssues = new ObservableCollection<HealthIssueDisplayModel>(healthIssues);
        }
    }
}
