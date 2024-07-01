using CommunityToolkit.Mvvm.ComponentModel;
using HealthProject.Models;

namespace HealthProject.ViewModels
{
    public partial class HospitalDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private HospitalDetailsModel hospital;

        public HospitalDetailsViewModel(HospitalDetailsModel hospital)
        {
            Hospital = hospital;
        }
    }
}
