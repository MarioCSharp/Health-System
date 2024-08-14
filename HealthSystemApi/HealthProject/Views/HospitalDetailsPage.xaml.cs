using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.DoctorService;
using HealthProject.ViewModels;
using Newtonsoft.Json;
namespace HealthProject.Views;

[QueryProperty(nameof(HospitalJson), "hospitalJson")]
public partial class HospitalDetailsPage : ContentPage
{
    private string hospitalJson;
    private readonly IDoctorService doctorService;
    private readonly IAuthenticationService authenticationService;
    public string HospitalJson
    {
        get => hospitalJson;
        set
        {
            hospitalJson = value;
            var hospital = JsonConvert.DeserializeObject<HospitalDetailsModel>(Uri.UnescapeDataString(value));
            BindingContext = new HospitalDetailsViewModel(hospital, doctorService, authenticationService);
        }
    }
    public HospitalDetailsPage(IDoctorService doctorService,
                               IAuthenticationService authenticationService)
	{
		InitializeComponent();

        this.doctorService = doctorService;
        this.authenticationService = authenticationService;
    }

    private async void OnAddDoctorClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(AddDoctorPage)}");
    }
}