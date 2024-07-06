using HealthProject.Models;
using HealthProject.Services.DoctorService;
using HealthProject.Services.ServiceService;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject;

[QueryProperty(nameof(DoctorJson), "doctorJson")]
public partial class AddServicePage : ContentPage
{
    private string doctorJson;
    private IServiceService serviceService;
    private IDoctorService doctorService;
    public string DoctorJson
    {
        get => doctorJson;
        set
        {
            doctorJson = value;
            var doctor = JsonConvert.DeserializeObject<DoctorPassModel>(Uri.UnescapeDataString(value));
            BindingContext = new AddServicePageViewModel(doctor, serviceService, doctorService);
        }
    }
    public AddServicePage(IServiceService serviceService,
                          IDoctorService doctorService)
	{
		InitializeComponent();
        this.serviceService = serviceService;
        this.doctorService = doctorService;
	}
}