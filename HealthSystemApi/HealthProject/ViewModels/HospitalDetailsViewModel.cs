using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.DoctorService;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HealthProject.Views;
namespace HealthProject.ViewModels
{
    public partial class HospitalDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private HospitalDetailsModel hospital;

        [ObservableProperty]
        private ObservableCollection<DoctorModel> doctors;

        private IDoctorService doctorService;
        public ICommand NavigateToDoctorDetailCommand { get; }

        public HospitalDetailsViewModel(HospitalDetailsModel hospital,
                                        IDoctorService doctorService)
        {
            Hospital = hospital;
            this.doctorService = doctorService;
            this.NavigateToDoctorDetailCommand = new AsyncRelayCommand<object>(DoctorDetailsAsync);
            LoadDoctors();
        }

        public async void LoadDoctors()
        {
            var doctorModels = await doctorService.AllAsync(hospital.Id);
            Doctors = new ObservableCollection<DoctorModel>(doctorModels);
        }

        public async Task DoctorDetailsAsync(object parameter)
        {
            if (parameter is int id)
            {
                var doctor = await doctorService.DetailsAsync(id);

                var doctorJson = JsonConvert.SerializeObject(doctor);
                var encodedDoctorJson = Uri.EscapeDataString(doctorJson);

                await Shell.Current.GoToAsync($"DoctorDetailsPage?doctorJson={encodedDoctorJson}");
            }
        }
    }
}
