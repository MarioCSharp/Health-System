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

        private IHospitalService hospitalService;
        private IAuthenticationService authenticationService;

        [ObservableProperty]
        private bool isFilterVisible;

        [ObservableProperty]
        private bool isSortVisible;

        [ObservableProperty]
        private ObservableCollection<string> filterOptions;

        [ObservableProperty]
        private ObservableCollection<string> sortOptions;

        [ObservableProperty]
        private string selectedFilterOption;

        [ObservableProperty]
        private string selectedSortOption;

        public HomePageViewModel(IHospitalService hospitalService,
                                 IAuthenticationService authenticationService)
        {
            this.hospitalService = hospitalService;
            this.authenticationService = authenticationService;

            ToggleFilterCommand = new RelayCommand(ToggleFilter);
            ToggleSortCommand = new RelayCommand(ToggleSort);
            NavigateToHospitalDetailCommand = new AsyncRelayCommand<object>(DetailsAsync);

            LoadHospitals();
            LoadFilterAndSortOptions();
        }

        public ICommand ToggleFilterCommand { get; }
        public ICommand ToggleSortCommand { get; }
        public ICommand NavigateToHospitalDetailCommand { get; }

        public async void LoadHospitals()
        {
            var hospitalModels = await hospitalService.All();
            Hospitals = new ObservableCollection<HospitalModel>(hospitalModels);
        }

        private void LoadFilterAndSortOptions()
        {
            FilterOptions = new ObservableCollection<string>
            {
                "Всички",
                "Option 1",
                "Option 2"
            };

            SortOptions = new ObservableCollection<string>
            {
                "Име",
                "Дата на добавяне ↑",
                "Дата на добавяне ↓"
            };
        }

        private void ToggleFilter()
        {
            IsFilterVisible = !IsFilterVisible;
            IsSortVisible = false;
        }

        private void ToggleSort()
        {
            IsSortVisible = !IsSortVisible;
            IsFilterVisible = false; 
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
