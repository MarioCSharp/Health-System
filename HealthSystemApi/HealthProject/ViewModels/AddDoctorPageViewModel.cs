using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.DoctorService;
using HealthProject.Services.HospitalService;
using Newtonsoft.Json;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class AddDoctorPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private AddDoctorModel doctorModel;

        public ICommand AddDoctorCommand { get; }
        public ICommand NavigateBackCommand { get; }

        private IDoctorService doctorService;
        private IHospitalService hospitalService;
        public AddDoctorPageViewModel(IDoctorService doctorService,
                                      IHospitalService hospitalService)
        {
            doctorModel = new AddDoctorModel();
            AddDoctorCommand = new AsyncRelayCommand(AddDoctorAsync);
            NavigateBackCommand = new Command(OnNavigateBack);
            this.doctorService = doctorService;
            this.hospitalService = hospitalService;
        }

        private async Task AddDoctorAsync()
        {
            var result = await doctorService.AddDoctorAsync(doctorModel);

            if (!result)
            {
                await Shell.Current.GoToAsync($"//{nameof(AddDoctorPage)}");
            }

            var hospital = await hospitalService.Details(doctorModel.HospitalId);
            var hospitalJson = JsonConvert.SerializeObject(hospital);
            var encodedHospitalJson = Uri.EscapeDataString(hospitalJson);
            await Shell.Current.GoToAsync($"///HospitalDetailsPage?hospitalJson={encodedHospitalJson}");
        }

        private async void OnNavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
