using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.DoctorService;
using HealthProject.Services.HospitalService;
using HealthProject.Views;
using Newtonsoft.Json;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class AddDoctorPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private AddDoctorModel doctorModel;

        private IDoctorService doctorService;
        private IHospitalService hospitalService;

        public AddDoctorPageViewModel(IDoctorService doctorService,
                                      IHospitalService hospitalService)
        {
            doctorModel = new AddDoctorModel();
            NavigateBackCommand = new Command(OnNavigateBack);
            this.doctorService = doctorService;
            this.hospitalService = hospitalService;
        }
        public ICommand AddDoctorCommand { get; }
        public ICommand NavigateBackCommand { get; }

        private async void OnNavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
