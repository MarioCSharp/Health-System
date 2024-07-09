using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using Newtonsoft.Json;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class ServiceDetailsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ServiceDetailsModel service;

        public ServiceDetailsPageViewModel(ServiceDetailsModel service)
        {
            Service = service;
            MakeBookingCommand = new AsyncRelayCommand<object>(OnMakeBooking);
        }

        public ICommand MakeBookingCommand { get; }

        private async Task OnMakeBooking(object parameter)
        {
            if (parameter is int id)
            {
                var model = new ServiceModel()
                {
                    Id = service.Id,
                    Name = service.Name,
                    Price = service.Price
                };

                var serviceJson = JsonConvert.SerializeObject(model);
                var encodedServiceJson = Uri.EscapeDataString(serviceJson);
                await Shell.Current.GoToAsync($"///BookingPage?serviceJson={encodedServiceJson}");
            }
        }
    }
}
