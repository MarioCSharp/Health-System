using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AppointmentService;
using HealthProject.Services.AuthenticationService;
using System;
using HealthProject.Views;

namespace HealthProject.ViewModels
{
    public partial class MyPrescriptionsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<PrescriptionDisplayModel> prescriptions;

        private readonly IAppointmentService appointmentService;
        private readonly IAuthenticationService authenticationService;

        public ICommand DownloadFileCommand { get; }

        public event Action<byte[]> DownloadFileRequested;

        public MyPrescriptionsViewModel(IAppointmentService appointmentService,
                                        IAuthenticationService authenticationService)
        {
            this.appointmentService = appointmentService;
            this.authenticationService = authenticationService;

            DownloadFileCommand = new RelayCommand<byte[]>(OnDownloadFile);
        }

        public async void LoadPrescriptions()
        {
            var token = await authenticationService.IsAuthenticated();

            if (!token.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

            var pres = await appointmentService.GetUserPrescriptions(token.UserId);
            Prescriptions = new ObservableCollection<PrescriptionDisplayModel>(pres);
        }

        private void OnDownloadFile(byte[] file)
        {
            DownloadFileRequested?.Invoke(file);
        }
    }
}
