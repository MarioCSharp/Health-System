using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.HospitalService;
using HealthProject.Services.NavigationService;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<HospitalModel> hospitals;

        private IHospitalService hospitalService;
        private INavigationService navigationService;
        public ICommand DeleteHospitalCommand { get; }
        public ICommand NavigateToHospitalDetailCommand { get; }
        public HomePageViewModel(IHospitalService hospitalService,
                                 INavigationService navigationService)
        {
            this.hospitalService = hospitalService;
            this.navigationService = navigationService;
            DeleteHospitalCommand = new AsyncRelayCommand<object>(DeleteAsync);
            NavigateToHospitalDetailCommand = new AsyncRelayCommand<object>(DetailsAsync);
            LoadHospitals();
        }

        public async void LoadHospitals()
        {
            var hospitalModels = await hospitalService.All();
            Hospitals = new ObservableCollection<HospitalModel>(hospitalModels);
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
