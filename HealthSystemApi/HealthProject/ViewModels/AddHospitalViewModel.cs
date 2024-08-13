using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.HospitalService;
using HealthProject.Views;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class AddHospitalViewModel : ObservableObject
    {
        [ObservableProperty]
        private AddHospitalModel hospitalModel;

        private IHospitalService hospitalService;
        public AddHospitalViewModel(IHospitalService hospitalService)
        {
            this.hospitalService = hospitalService;
            hospitalModel = new AddHospitalModel();
            AddHospitalCommand = new AsyncRelayCommand(AddHospitalCommandAsync);
            NavigateBackCommand = new Command(OnNavigateBack);
        }

        public ICommand AddHospitalCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public async Task AddHospitalCommandAsync()
        {
            await hospitalService.Add(HospitalModel);

            await Shell.Current.GoToAsync($"{nameof(HomePage)}");
        }

        private async void OnNavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
