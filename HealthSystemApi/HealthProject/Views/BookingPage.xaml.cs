using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.ServiceService;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject.Views;

[QueryProperty(nameof(ServiceJson), "serviceJson")]
public partial class BookingPage : ContentPage
{
    private string serviceJson;
    private IServiceService serviceService;
    public string ServiceJson
    {
        get => serviceJson;
        set
        {
            serviceJson = value;
            var service = JsonConvert.DeserializeObject<ServiceModel>(Uri.UnescapeDataString(value));
            BindingContext = new BookingViewModel(service, serviceService);
        }
    }
    public BookingPage(IServiceService serviceService)
	{
        InitializeComponent();
        this.serviceService = serviceService;
	}
}