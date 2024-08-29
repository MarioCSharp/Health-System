using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.HospitalService;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class ReceptionViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<HospitalModel> hospitals;

        private IHospitalService hospitalService;
        private IAuthenticationService authenticationService;
        public ReceptionViewModel(IHospitalService hospitalService,
                                  IAuthenticationService authenticationService)
        {
            this.hospitalService = hospitalService;
            this.authenticationService = authenticationService;

            ConnectToReceptionChatCommand = new AsyncRelayCommand<object>(ConnectAsync);

            LoadHospitals();
        }

        public ICommand ConnectToReceptionChatCommand { get; }

        public async void LoadHospitals()
        {
            var hospitalModels = await hospitalService.All();

            Hospitals = new ObservableCollection<HospitalModel>(hospitalModels);
        }

        public async Task ConnectAsync(object parameter)
        {
            if (parameter is int id)
            {
                await Shell.Current.GoToAsync($"ReceptionChatPage?id={id}");
                return;
            }
        }
    }
}
