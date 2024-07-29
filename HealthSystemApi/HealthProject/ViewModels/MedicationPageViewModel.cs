using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.MedicationService;
using HealthProject.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class MedicationPageViewModel : ObservableObject
    {
        private IMedicationService medicationService;
        private IAuthenticationService authenticationService;

        [ObservableProperty]
        private ObservableCollection<MedicationDisplayModel> medications;

        public MedicationPageViewModel(IMedicationService medicationService,
                                       IAuthenticationService authenticationService)
        {
            this.medicationService = medicationService;
            this.authenticationService = authenticationService;

            this.RedirectToAddPageCommand = new AsyncRelayCommand(RedirectToAddAsync);
            this.RedirectToDetailsPageCommand = new AsyncRelayCommand<object>(RedirectToDetailsAsync);

            LoadMedications();
        }

        public ICommand RedirectToAddPageCommand { get; }
        public ICommand RedirectToDetailsPageCommand { get; }

        private async void LoadMedications()
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (!authToken.IsAuthenticated) 
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            var meds = await medicationService.AllByUser(authToken.UserId);

            Medications = new ObservableCollection<MedicationDisplayModel>(meds);
        }

        private async Task RedirectToAddAsync()
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (!authToken.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            await Shell.Current.GoToAsync($"//{nameof(MedicationAddPage)}");
        }

        private async Task RedirectToDetailsAsync(object parameter)
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (!authToken.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            if (parameter is int id)
            {
                var medication = await medicationService.DetailsAsync(id);

                var medJson = JsonConvert.SerializeObject(medication);
                var encodedMedicationJson = Uri.EscapeDataString(medJson);

                await Shell.Current.GoToAsync($"//{nameof(MedicationDetailsPage)}?medicationJson={encodedMedicationJson}");
            }
        }
    }
}
