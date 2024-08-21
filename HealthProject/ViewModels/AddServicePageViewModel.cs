using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.DoctorService;
using HealthProject.Services.ServiceService;
using Newtonsoft.Json;
using System.Windows.Input;
namespace HealthProject.ViewModels
{
    public partial class AddServicePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private DoctorPassModel doctorPass;

        [ObservableProperty]
        private ServiceAddModel service;

        private IServiceService serviceService;
        private IDoctorService doctorService;

        public AddServicePageViewModel(DoctorPassModel doctorPass,
                                       IServiceService serviceService,
                                       IDoctorService doctorService)
        {
            DoctorPass = doctorPass;
            this.serviceService = serviceService;
            this.doctorService = doctorService;
            service = new ServiceAddModel() { DoctorId = doctorPass.Id };
        }
        public ICommand AddServiceCommand { get; }

    }
}
