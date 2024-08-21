using CommunityToolkit.Mvvm.ComponentModel;
using HealthProject.Models;
using HealthProject.Services.LogbookService;
using HealthProject.Views;

namespace HealthProject.ViewModels
{
    public partial class LogbookEditViewModel : ObservableObject
    {
        [ObservableProperty]
        private LogAddModel log;

        private ILogbookService logbookService;

        public LogbookEditViewModel(LogAddModel log,
                                    ILogbookService logbookService)
        {
            this.logbookService = logbookService;
            Log = log;
        }

        public async Task EditAsync(List<int> values, List<string> factors, string notes, int hId, string type)
        {
            var log = new LogAddModel()
            {
                Date = DateTime.Now,
                Values = values,
                Factors = factors,
                Note = notes,
                HealthIssueId = hId,
                Type = type
            };

            var res = await logbookService.EditAsync(log);

            if (res)
            {
                await Shell.Current.GoToAsync($"//{nameof(LogbookViewPage)}");
            }
        }
    }
}
