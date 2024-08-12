using CommunityToolkit.Mvvm.ComponentModel;
using HealthProject.Models;
using HealthProject.Services.ServiceService;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Services.AuthenticationService;
using HealthProject.Views;
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

        [ObservableProperty]
        private string selectedHour;

        private IServiceService serviceService;
        private IAuthenticationService authenticationService;
        public BookingViewModel(ServiceModel service,
                                IServiceService serviceService,
                                IAuthenticationService authenticationService)
        {
            this.service = service;
            this.authenticationService = authenticationService;
            this.serviceService = serviceService;
            SelectedDate = DateTime.Today;
            AvailableHours = new ObservableCollection<string>();
            LoadAvailableHours(SelectedDate);

            MakeBookingCoammand = new AsyncRelayCommand(BookAsync);
        }

        public ICommand MakeBookingCoammand { get; }

        partial void OnSelectedDateChanged(DateTime value)
        {
            LoadAvailableHours(value);
        }

        private async void LoadAvailableHours(DateTime date)
        {
            var hours = await serviceService.AvailableHoursAsync(date, service.Id);

            AvailableHours = new ObservableCollection<string>(hours);
        }

        public async Task BookAsync()
        {
            var authToken = await authenticationService.IsAuthenticated();

            if (!authToken.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }

            if (!string.IsNullOrEmpty(SelectedHour))
            {
                var model = new MakeBookingModel();

                model.UserId = authToken.UserId;
                model.ServiceId = Service.Id;
                model.DoctorId = 0;
                model.Time = SelectedHour;
                model.Day = SelectedDate.Day;
                model.Month = SelectedDate.Month;
                model.Year = SelectedDate.Year;

                var result = await serviceService.BookAsync(model);

                if (result)
                {
                    await Shell.Current.GoToAsync($"//{nameof(AppointmentHistoryPage)}");
                    return;
                }
            }
        }
    }
}
