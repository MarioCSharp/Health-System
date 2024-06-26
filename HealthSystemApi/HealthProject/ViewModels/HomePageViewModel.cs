using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.HospitalService;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<HospitalModel> hospitals;

        private IHospitalService hospitalService;
        public ICommand DeleteHospitalCommand { get; }
        public HomePageViewModel(IHospitalService hospitalService)
        {
            this.hospitalService = hospitalService;
            DeleteHospitalCommand = new AsyncRelayCommand<object>(DeleteAsync);
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
    }
}
