using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.ServiceService;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class DoctorDetailsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private DoctorDetailsModel doctor;

        public ICommand EditDoctorInfoRedirect { get; }
        public ICommand NavigateBackCommand { get; }
        public ICommand AddServiceRedirect { get; }
        public ICommand ServiceDetailsCommand { get; }
        private IServiceService serviceService;

        [ObservableProperty]
        private ObservableCollection<ServiceModel> services;

        public DoctorDetailsPageViewModel(DoctorDetailsModel doctor,
                                          IServiceService serviceService)
        {
            Doctor = doctor;
            EditDoctorInfoRedirect = new AsyncRelayCommand<object>(RedirectToEditInfo);
            NavigateBackCommand = new AsyncRelayCommand(OnNavigateBack);
            AddServiceRedirect = new AsyncRelayCommand<object>(RedirectToAddService);
            ServiceDetailsCommand = new AsyncRelayCommand<object>(ServiceDetailsAsync);
            this.serviceService = serviceService;
            LoadServices();
        }

        private async void LoadServices()
        {
            var servicesList = await serviceService.AllByIdAsync(doctor.Id);
            Services = new ObservableCollection<ServiceModel>(servicesList);
        }

        public async Task RedirectToEditInfo(object parameter)
        {
            if (parameter is int id)
            {
                var doctorJson = JsonConvert.SerializeObject(doctor);
                var encodedDoctorJson = Uri.EscapeDataString(doctorJson);
                await Shell.Current.GoToAsync($"///EditDoctorInfo?hospitalJson={encodedDoctorJson}");
            }
        }

        private async Task OnNavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async Task RedirectToAddService(object parameter)
        {
            if (parameter is int id)
            {
                var doctorJson = JsonConvert.SerializeObject(new DoctorPassModel { Id = id });
                var encodedDoctorJson = Uri.EscapeDataString(doctorJson);
                await Shell.Current.GoToAsync($"///AddServicePage?doctorJson={encodedDoctorJson}");
            }
        }

        private async Task ServiceDetailsAsync(object parameter)
        {
            if (parameter is int id)
            {
                var service = await serviceService.DetailsAsync(id);
                var serviceJson = JsonConvert.SerializeObject(service);
                var encodedServiceJson = Uri.EscapeDataString(serviceJson);
                await Shell.Current.GoToAsync($"///ServiceDetailsPage?serviceJson={encodedServiceJson}");
            }
        }
    }
}
