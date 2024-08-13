using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.DoctorService;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace HealthProject.ViewModels
{
    public partial class HospitalDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private HospitalDetailsModel hospital;

        [ObservableProperty]
        private ObservableCollection<DoctorModel> doctors;

        private IDoctorService doctorService;

        public HospitalDetailsViewModel(HospitalDetailsModel hospital,
                                        IDoctorService doctorService)
        {
            Hospital = hospital;
            this.doctorService = doctorService;
            NavigateToDoctorDetailCommand = new AsyncRelayCommand<object>(DoctorDetailsAsync);
            LoadDoctors();
        }

        public ICommand NavigateToDoctorDetailCommand { get; }

        public async void LoadDoctors()
        {
            var doctorModels = await doctorService.AllAsync(Hospital.Id);
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
