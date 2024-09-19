using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.ServiceService;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject.Views;

[QueryProperty(nameof(DoctorJson), "doctorJson")]
public partial class DoctorDetailsPage : ContentPage
{
    private string doctorJson;
    private IServiceService serviceService;
    private IAuthenticationService authenticationService;
    public string DoctorJson
    {
        get => doctorJson;
        set
        {
            doctorJson = value;
            var hospital = JsonConvert.DeserializeObject<DoctorDetailsModel>(Uri.UnescapeDataString(value));
            BindingContext = new DoctorDetailsPageViewModel(hospital, serviceService, authenticationService);
        }
    }

    public DoctorDetailsPage(IServiceService serviceService,
                             IAuthenticationService authenticationService)
	{
		InitializeComponent();

        Title = "Доктор";

        this.serviceService = serviceService;
        this.authenticationService = authenticationService;
    }
}