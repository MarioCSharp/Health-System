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

        private IHealthIssueService healthIssueService;

        public HealthIssueAddViewModel(IHealthIssueService healthIssueService,
                                       string? userId)
        {
            HealthIssue = new HealthIssueAddModel();
            this.healthIssueService = healthIssueService;
            AddHealthIssueCommand = new AsyncRelayCommand(AddHealthIssueAsync);
            this.userId = userId; 
            healthIssue.IssueEndDate = DateTime.Now;
        }
        public ICommand AddHealthIssueCommand { get; }

        public async Task AddHealthIssueAsync()
        {
            var result = await healthIssueService.AddAsync(HealthIssue, UserId ?? string.Empty);

            if (!result)
            {
                return;
            }

            await Shell.Current.GoToAsync($"///{nameof(HealthIssuesPage)}");
        }
    }
}
