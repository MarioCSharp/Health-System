using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.HealthIssueService;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class HealthIssueAddViewModel : ObservableObject
    {
        [ObservableProperty]
        private HealthIssueAddModel healthIssue;

        public ICommand AddHealthIssueCommand { get; }

        private IHealthIssueService healthIssueService;

        public HealthIssueAddViewModel(IHealthIssueService healthIssueService)
        {
            HealthIssue = new HealthIssueAddModel();
            this.healthIssueService = healthIssueService;
            AddHealthIssueCommand = new AsyncRelayCommand(AddHealthIssueAsync);
        }

        public async Task AddHealthIssueAsync()
        {
            var result = await healthIssueService.AddAsync(healthIssue);

            if (!result)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(HealthIssuesPage)}");
        }
    }
}
