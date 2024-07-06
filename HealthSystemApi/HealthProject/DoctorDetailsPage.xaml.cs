using HealthProject.Models;
using HealthProject.Services.ServiceService;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject;

[QueryProperty(nameof(DoctorJson), "doctorJson")]
public partial class DoctorDetailsPage : ContentPage
{
    private string doctorJson;
    private IServiceService serviceService;
    public string DoctorJson
    {
        get => doctorJson;
        set
        {
            doctorJson = value;
            var hospital = JsonConvert.DeserializeObject<DoctorDetailsModel>(Uri.UnescapeDataString(value));
            BindingContext = new DoctorDetailsPageViewModel(hospital, serviceService);
        }
    }
    public DoctorDetailsPage(IServiceService serviceService)
	{
		InitializeComponent();
        this.serviceService = serviceService;
    }
}