using CommunityToolkit.Mvvm.ComponentModel;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.ServiceService;
using System.Collections.ObjectModel;

namespace HealthProject.ViewModels
{
    public partial class BookingViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime selectedDate;

        [ObservableProperty]
        private ObservableCollection<string> availableHours;

        [ObservableProperty]
        private ServiceModel service;

        private IServiceService serviceService;
        public BookingViewModel(ServiceModel service,
                                IServiceService serviceService)
        {
            this.service = service;
            this.serviceService = serviceService;
            SelectedDate = DateTime.Today;
            AvailableHours = new ObservableCollection<string>();
            LoadAvailableHours(SelectedDate);
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            LoadAvailableHours(value);
        }

        private async void LoadAvailableHours(DateTime date)
        {
            var hours = await serviceService.AvailableHoursAsync(date, service.Id);

            AvailableHours = new ObservableCollection<string>(hours);
        }
    }
}
