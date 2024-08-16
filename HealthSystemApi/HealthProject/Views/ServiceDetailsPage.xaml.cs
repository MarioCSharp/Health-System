using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject.Views;

[QueryProperty(nameof(ServiceJson), "serviceJson")]
public partial class ServiceDetailsPage : ContentPage
{
    private string serviceJson;
    private IAuthenticationService authenticationService;
    public string ServiceJson
    {
        get => serviceJson;
        set
        {
            serviceJson = value;
            var service = JsonConvert.DeserializeObject<ServiceDetailsModel>(Uri.UnescapeDataString(value));
            BindingContext = new ServiceDetailsPageViewModel(service, authenticationService);
        }
    }
    public ServiceDetailsPage(IAuthenticationService authenticationService)
	{
		InitializeComponent();

        Title = "Детайли за услуга";

        this.authenticationService = authenticationService;
	}
}