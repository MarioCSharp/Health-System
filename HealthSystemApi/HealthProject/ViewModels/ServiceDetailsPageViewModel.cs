using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthProject.Models;
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
        }
    }
}
