using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.HospitalService;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class AddHospitalViewModel : ObservableObject
    {
        [ObservableProperty]
        private AddHospitalModel hospitalModel;
        public ICommand AddHospitalCommand { get; }
        public ICommand NavigateBackCommand { get; }
        private IHospitalService hospitalService;
        public AddHospitalViewModel(IHospitalService hospitalService)
        {
            this.hospitalService = hospitalService;
            hospitalModel = new AddHospitalModel();
            AddHospitalCommand = new AsyncRelayCommand(AddHospitalCommandAsync);
            NavigateBackCommand = new Command(OnNavigateBack);
        }

        public async Task AddHospitalCommandAsync()
        {
            await hospitalService.Add(hospitalModel);

            await Shell.Current.GoToAsync($"{nameof(HomePage)}");
        }

        private async void OnNavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
