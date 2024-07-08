using HealthProject.Models;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject;

[QueryProperty(nameof(ServiceJson), "serviceJson")]
public partial class ServiceDetailsPage : ContentPage
{
    private string serviceJson;
    public string ServiceJson
    {
        get => serviceJson;
        set
        {
            serviceJson = value;
            var service = JsonConvert.DeserializeObject<ServiceDetailsModel>(Uri.UnescapeDataString(value));
            BindingContext = new ServiceDetailsPageViewModel(service);
        }
    }
    public ServiceDetailsPage()
	{
		InitializeComponent();
	}
}