using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.LogbookService;
using HealthProject.Views;
using System.Windows.Input;

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
            Log = log;
            this.logbookService = logbookService;

            EditCommand = new AsyncRelayCommand(EditAsync);
        }

        public ICommand EditCommand { get; }

        public async Task EditAsync()
        {
            var res = await logbookService.EditAsync(log);

            if (res)
            {
                await Shell.Current.GoToAsync($"//{nameof(LogbookViewPage)}");
            }
        }
    }
}
