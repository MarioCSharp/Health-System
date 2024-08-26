using CommunityToolkit.Mvvm.ComponentModel;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.LogbookService;
using HealthProject.Views;

namespace HealthProject.ViewModels
{
    public partial class LogbookAddViewModel : ObservableObject
    {
        private ILogbookService logbookService;
        private IAuthenticationService authenticationService;

        public LogbookAddViewModel(ILogbookService logbookService,
                                   IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
            this.logbookService = logbookService;
        }

        public async Task AddAsync(List<int> values, List<string> factors, string notes, string type)
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (!authToken.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            var log = new LogAddModel()
            {
                Values = values,
                Factors = factors,
                Note = notes,
                Date = DateTime.Now,
                Type = type,
                UserId = authToken.UserId
            };

            var result = await logbookService.AddAsync(log);

            if (result) 
            {
                await Shell.Current.GoToAsync($"///{nameof(LogbookViewPage)}");
            }
        }
    }
}
