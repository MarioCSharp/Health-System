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
        [ObservableProperty]
        private ObservableCollection<MedicationDisplayModel> medications;

        private IMedicationService medicationService;
        private IAuthenticationService authenticationService;

        public MedicationPageViewModel(IMedicationService medicationService,
                                       IAuthenticationService authenticationService)
        {
            this.medicationService = medicationService;
            this.authenticationService = authenticationService;

            RedirectToAddPageCommand = new AsyncRelayCommand(RedirectToAddAsync);
            RedirectToDetailsPageCommand = new AsyncRelayCommand<object>(RedirectToDetailsAsync);
            DeleteMedicationCommand = new AsyncRelayCommand<object>(DeleteAsync);

            LoadMedications();
        }

        public ICommand RedirectToAddPageCommand { get; }
        public ICommand RedirectToDetailsPageCommand { get; }
        public ICommand DeleteMedicationCommand { get; }

        public async Task LoadMedications()
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (!authToken.IsAuthenticated) 
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            var meds = await medicationService.AllByUser(authToken.UserId ?? string.Empty);

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

            await Shell.Current.GoToAsync($"{nameof(MedicationAddPage)}");
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

                await Shell.Current.GoToAsync($"{nameof(MedicationDetailsPage)}?medicationJson={encodedMedicationJson}");
            }
        }

        private async Task DeleteAsync(object parameter)
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (!authToken.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            if (parameter is int id)
            {
                await medicationService.DeleteAsync(id);
                await LoadMedications();
                await Shell.Current.GoToAsync($"//{nameof(MedicationViewPage)}");
            }
        }
    }
}
