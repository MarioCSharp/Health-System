using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.MedicationService;
using HealthProject.Views;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class MedicationAddViewModel : ObservableObject
    {
        private IMedicationService medicationService;
        private IAuthenticationService authenticationService;

        [ObservableProperty]
        private MedicationAddModel medication;

        public MedicationAddViewModel(IMedicationService medicationService,
                                      IAuthenticationService authenticationService)
        {
            this.medicationService = medicationService;
            this.authenticationService = authenticationService;
            this.Medication = new MedicationAddModel();
        }

        public async Task AddAsync(List<TimeSpan> times, int skipCount, List<DayOfWeek> days, int take, int rest)
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (!authToken.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }

            Medication.UserId = authToken.UserId;
            Medication.Times = times;
            Medication.Days = days;
            Medication.Take = take;
            Medication.Rest = rest;
            Medication.SkipCount = skipCount;

            await medicationService.AddAsync(Medication);

            await Shell.Current.GoToAsync($"//{nameof(MedicationViewPage)}");
        }
    }
}
