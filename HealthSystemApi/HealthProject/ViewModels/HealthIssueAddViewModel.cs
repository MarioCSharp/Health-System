using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.HealthIssueService;
using System.Windows.Input;
using HealthProject.Views;
using HealthProject.Services.AuthenticationService;
namespace HealthProject.ViewModels
{
    public partial class HealthIssueAddViewModel : ObservableObject
    {
        [ObservableProperty]
        private HealthIssueAddModel healthIssue;

        [ObservableProperty]
        private string? userId;

        private IHealthIssueService healthIssueService;
        private IAuthenticationService authenticationService;

        public HealthIssueAddViewModel(IHealthIssueService healthIssueService,
                                       string? userId,
                                       IAuthenticationService authenticationService)
        {
            HealthIssue = new HealthIssueAddModel();
            this.healthIssueService = healthIssueService;
            AddHealthIssueCommand = new AsyncRelayCommand(AddHealthIssueAsync);
            this.userId = userId; 
            this.authenticationService = authenticationService;
            healthIssue.IssueEndDate = DateTime.Now;
        }
        public ICommand AddHealthIssueCommand { get; }

        public async Task AddHealthIssueAsync()
        {
            var auth = await authenticationService.IsAuthenticated();

            if (!auth.IsAuthenticated) 
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            var result = await healthIssueService.AddAsync(HealthIssue, auth.UserId ?? "");

            if (!result)
            {
                return;
            }

            await Shell.Current.GoToAsync($"///{nameof(HealthIssuesPage)}");
        }
    }
}
