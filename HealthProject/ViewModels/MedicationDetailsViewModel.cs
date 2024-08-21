using CommunityToolkit.Mvvm.ComponentModel;
using HealthProject.Models;

namespace HealthProject.ViewModels
{
    public partial class MedicationDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private MedicationDetailsModel medication;

        public MedicationDetailsViewModel(MedicationDetailsModel medication)
        {
            this.Medication = medication;
        }
    }
}
