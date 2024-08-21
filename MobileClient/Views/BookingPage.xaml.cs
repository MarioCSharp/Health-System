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
    private IAuthenticationService authenticationService;
    public string ServiceJson
    {
        get => serviceJson;
        set
        {
            serviceJson = value;
            var service = JsonConvert.DeserializeObject<ServiceModel>(Uri.UnescapeDataString(value));
            BindingContext = new BookingViewModel(service, serviceService, authenticationService);
        }
    }
    public BookingPage(IServiceService serviceService,
                       IAuthenticationService authenticationService)
	{
        InitializeComponent();

        Title = "Записване на час";

        this.serviceService = serviceService;
        this.authenticationService = authenticationService;
	}
}