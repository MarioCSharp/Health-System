using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.HealthIssueService;
using System.Windows.Input;
using HealthProject.Views;
namespace HealthProject.ViewModels
{
    public partial class HealthIssueAddViewModel : ObservableObject
    {
        [ObservableProperty]
        private HealthIssueAddModel healthIssue;

        [ObservableProperty]
        private string? userId;

        public ICommand AddHealthIssueCommand { get; }

        private IHealthIssueService healthIssueService;
        public HealthIssueAddViewModel(IHealthIssueService healthIssueService,
            string? userId)
        {
            HealthIssue = new HealthIssueAddModel();
            this.healthIssueService = healthIssueService;
            AddHealthIssueCommand = new AsyncRelayCommand(AddHealthIssueAsync);
            this.userId = userId; 
        }

        public async Task AddHealthIssueAsync()
        {
            var result = await healthIssueService.AddAsync(healthIssue, userId);

            if (!result)
            {
                return;
            }

            await Shell.Current.GoToAsync($"///{nameof(HealthIssuesPage)}");
        }
    }
}
