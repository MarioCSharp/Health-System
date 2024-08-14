using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.HospitalService;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HealthProject.Views;
using HealthProject.Services.AuthenticationService;
namespace HealthProject.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<HospitalModel> hospitals;

        [ObservableProperty]
        private bool isAdmin;

        private IHospitalService hospitalService;
        private IAuthenticationService authenticationService;

        public HomePageViewModel(IHospitalService hospitalService,
                                 IAuthenticationService authenticationService)
        {
            this.hospitalService = hospitalService;
            this.authenticationService = authenticationService;

            DeleteHospitalCommand = new AsyncRelayCommand<object>(DeleteAsync);
            NavigateToHospitalDetailCommand = new AsyncRelayCommand<object>(DetailsAsync);

            LoadHospitals();
            CheckAdmin();
        }

        public ICommand DeleteHospitalCommand { get; }
        public ICommand NavigateToHospitalDetailCommand { get; }

        public async void LoadHospitals()
        {
            var hospitalModels = await hospitalService.All();

            Hospitals = new ObservableCollection<HospitalModel>(hospitalModels);
        }

        public async void CheckAdmin()
        {
            IsAdmin = await authenticationService.IsAdmin();
        }

        public async Task DeleteAsync(object parameter)
        {
            if (parameter is int id)
            {
                await hospitalService.Delete(id);
                LoadHospitals();
            }
        }
        public async Task DetailsAsync(object parameter)
        {
            if (parameter is int id)
            {
                var hospital = await hospitalService.Details(id);
                var hospitalJson = JsonConvert.SerializeObject(hospital);
                var encodedHospitalJson = Uri.EscapeDataString(hospitalJson);
                await Shell.Current.GoToAsync($"{nameof(HospitalDetailsPage)}?hospitalJson={encodedHospitalJson}");
            }
        }
    }
}
