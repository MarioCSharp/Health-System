using CommunityToolkit.Mvvm.ComponentModel;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.MedicationService;
using HealthProject.Views;
using System.Collections.ObjectModel;

namespace HealthProject.ViewModels
{
    public partial class MedicationScheduleViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<MedicationScheduleModel> medications;

        [ObservableProperty]
        private ObservableCollection<MedicationScheduleModel> filteredMedications;

        [ObservableProperty]
        private DateTime selectedDate;

        private readonly IAuthenticationService authenticationService;
        private readonly IMedicationService medicationService;

        public MedicationScheduleViewModel(IAuthenticationService authenticationService,
                                           IMedicationService medicationService)
        {
            this.authenticationService = authenticationService;
            this.medicationService = medicationService;

            SelectedDate = DateTime.Today;
            LoadMedications();
        }

        public async void LoadMedications()
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (!authToken.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            var meds = await medicationService.SchedulesAsync(authToken.UserId ?? string.Empty);

            Medications = new ObservableCollection<MedicationScheduleModel>(meds);
            FilterMedications();
        }

        private void FilterMedications()
        {
            if (Medications == null) return;

            var filtered = Medications.Where(m => ShouldShowMedication(m, SelectedDate)).ToList();
            FilteredMedications = new ObservableCollection<MedicationScheduleModel>(filtered);
        }

        private bool ShouldShowMedication(MedicationScheduleModel medication, DateTime date)
        {
            if (date < medication.StartDate || date > medication.EndDate)
            {
                return false;
            }

            if (medication.SkipCount > 0)
            {
                var daysSinceStart = (date - medication.StartDate).Days;
                return daysSinceStart % medication.SkipCount == 0;
            }

            if (medication.Days.Any())
            {
                return medication.Days.Contains(date.DayOfWeek);
            }

            if (medication.Take > 0 && medication.Rest > 0)
            {
                var cycleLength = medication.Take + medication.Rest;
                var daysSinceStart = (date - medication.StartDate).Days;
                var cycleDay = daysSinceStart % cycleLength;
                return cycleDay < medication.Take;
            }

            return true;
        }

        public void OnDateSelected(DateTime date)
        {
            SelectedDate = date;
            FilterMedications();
        }
    }
}
