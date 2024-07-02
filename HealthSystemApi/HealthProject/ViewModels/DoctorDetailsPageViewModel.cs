using CommunityToolkit.Mvvm.ComponentModel;
using HealthProject.Models;

namespace HealthProject.ViewModels
{
    public partial class DoctorDetailsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private DoctorDetailsModel doctor;

        public DoctorDetailsPageViewModel(DoctorDetailsModel doctor)
        {
            Doctor = doctor;
        }
    }
}
