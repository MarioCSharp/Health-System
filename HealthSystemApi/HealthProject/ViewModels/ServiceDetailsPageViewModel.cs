using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using Newtonsoft.Json;
using System.Windows.Input;

namespace HealthProject.ViewModels
{
    public partial class ServiceDetailsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ServiceDetailsModel service;

        private IAuthenticationService authenticationService;
        public ServiceDetailsPageViewModel(ServiceDetailsModel service,
                                           IAuthenticationService authenticationService)
        {
            Service = service;
            this.authenticationService = authenticationService;
            MakeBookingCommand = new AsyncRelayCommand<object>(OnMakeBooking);
        }

        public ICommand MakeBookingCommand { get; }

        private async Task OnMakeBooking(object parameter)
        {
            var auth = await authenticationService.IsAuthenticated();
            if (auth.IsAuthenticated == false)
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                return;
            }

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
