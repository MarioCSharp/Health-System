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
        private IDoctorService doctorService;
        private IHospitalService hospitalService;

        public EditDoctorInfoViewModel(DoctorDetailsModel doctor,
                                       IDoctorService doctorService,
                                       IHospitalService hospitalService)
        {
            Doctor = doctor;
            this.doctorService = doctorService;
            this.hospitalService = hospitalService;
        }
        public ICommand SaveDoctorCommand { get; }

    }
}
