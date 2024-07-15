using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.DoctorService;
using HealthProject.Services.HospitalService;
using Newtonsoft.Json;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class EditDoctorInfoViewModel : ObservableObject
    {
        [ObservableProperty]
        private DoctorDetailsModel doctor;
        public ICommand SaveDoctorCommand { get; }
        private IDoctorService doctorService;
        private IHospitalService hospitalService;
        public EditDoctorInfoViewModel(DoctorDetailsModel doctor,
                                       IDoctorService doctorService,
                                       IHospitalService hospitalService)
        {
            Doctor = doctor;
            this.doctorService = doctorService;
            this.hospitalService = hospitalService;
            this.SaveDoctorCommand = new AsyncRelayCommand(SaveDoctorInfo);
        }

        public async Task SaveDoctorInfo()
        {
            await doctorService.Edit(doctor);

            var doc = await doctorService.GetDoctor(doctor.Id);
            var hospital = await hospitalService.Details(doc.HospitalId);
            var hospitalJson = JsonConvert.SerializeObject(hospital);
            var encodedHospitalJson = Uri.EscapeDataString(hospitalJson);
            await Shell.Current.GoToAsync($"HospitalDetailsPage?hospitalJson={encodedHospitalJson}");
        }
    }
}
