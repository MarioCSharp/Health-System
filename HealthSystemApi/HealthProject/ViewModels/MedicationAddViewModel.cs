using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.MedicationService;
using HealthProject.Views;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class MedicationAddViewModel : ObservableObject
    {
        private IMedicationService medicationService;

        [ObservableProperty]
        private MedicationAddModel medication;

        public MedicationAddViewModel(IMedicationService medicationService)
        {
            this.medicationService = medicationService;
            this.Medication = new MedicationAddModel();

            this.AddCommand = new AsyncRelayCommand(AddAsync);
        }

        public ICommand AddCommand { get; }

        private async Task AddAsync()
        {
            await medicationService.AddAsync(Medication);

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
