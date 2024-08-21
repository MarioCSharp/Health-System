using CommunityToolkit.Mvvm.ComponentModel;
using HealthProject.Models;
using HealthProject.Services.HealthIssueService;
using HealthProject.Views;
namespace HealthProject.ViewModels
{
    public partial class HealtIssueDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private HealthIssueDetailsModel healthIssue;

        private IHealthIssueService healthIssueService;

        public HealtIssueDetailsViewModel(HealthIssueDetailsModel healthIssue,
                                          IHealthIssueService healthIssueService)
        {
            HealthIssue = healthIssue;
            this.healthIssueService = healthIssueService;
        }
    }
}
