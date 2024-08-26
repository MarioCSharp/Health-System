using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Services.AuthenticationService;
using System.Windows.Input;
using HealthProject.Views;
using System.Collections.ObjectModel;
using HealthProject.Models;
using HealthProject.Services.AppointmentService;
using HealthProject.Services.MedicationService;

namespace HealthProject.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private IAuthenticationService authenticationService;
        private IAppointmentService appointmentService;
        private IMedicationService medicationService;

        [ObservableProperty]
        private ObservableCollection<AppointmentModel> appointments;

        [ObservableProperty]
        private ObservableCollection<MedicationDisplayModel> validMedications;

        public MainPageViewModel(IAuthenticationService authenticationService,
                                 IAppointmentService appointmentService,
                                 IMedicationService medicationService)
        {
            EnterCommand = new AsyncRelayCommand(EnterAsync);

            this.authenticationService = authenticationService;
            this.appointmentService = appointmentService;
            this.medicationService = medicationService;

            LoadResources();
        }
        public ICommand EnterCommand { get; }

        public async Task EnterAsync()
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (authToken.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }

        public async void LoadNextAppointments()
        {
            var nextAppointments = await appointmentService.GetUsersNextAppointments();

            Appointments = new ObservableCollection<AppointmentModel>(nextAppointments);
        }

        public async void LoadValidMedications()
        {
            var validMedications = await medicationService.GetUsersValidMedications();

            ValidMedications = new ObservableCollection<MedicationDisplayModel>(validMedications);
        }

        public async void LoadResources()
        {
            var auth = await authenticationService.IsAuthenticated();

            if (!auth.IsAuthenticated)
            {
                Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            LoadNextAppointments();
            LoadValidMedications();
        }
    }
}
