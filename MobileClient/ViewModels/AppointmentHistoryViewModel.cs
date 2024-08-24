using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AppointmentService;
using HealthProject.Services.AuthenticationService;
using HealthProject.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class AppointmentHistoryViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<AppointmentModel> history;

        private IAppointmentService appointmentService;
        private IAuthenticationService authenticationService;

        public AppointmentHistoryViewModel(IAppointmentService appointmentService,
                                           IAuthenticationService authenticationService)
        {
            this.appointmentService = appointmentService;
            this.authenticationService = authenticationService;

            AddRatingCommand = new AsyncRelayCommand<object>(RedirectToAddRating);

            LoadHistory();  
        }

        public ICommand AddRatingCommand { get; }

        public async Task RedirectToAddRating(object parameter)
        {
            if (parameter is int id)
            {
                await Shell.Current.GoToAsync($"AddRatingPage?id={id}");
                return;
            }
        }

        public async Task LoadHistory()
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (!authToken.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            var apps = await appointmentService.GetUserAppointmentsAsync(authToken.UserId);

            History = new ObservableCollection<AppointmentModel>(apps);
        }
    }
}
