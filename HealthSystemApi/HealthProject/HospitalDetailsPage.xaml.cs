using HealthProject.Models;
using HealthProject.Services.DoctorService;
using HealthProject.ViewModels;
using Newtonsoft.Json;
namespace HealthProject;

[QueryProperty(nameof(HospitalJson), "hospitalJson")]
public partial class HospitalDetailsPage : ContentPage
{
    private string hospitalJson;
    private readonly IDoctorService doctorService;
    public string HospitalJson
    {
        get => hospitalJson;
        set
        {
            hospitalJson = value;
            var hospital = JsonConvert.DeserializeObject<HospitalDetailsModel>(Uri.UnescapeDataString(value));
            BindingContext = new HospitalDetailsViewModel(hospital, doctorService);
        }
    }
    public HospitalDetailsPage(IDoctorService doctorService)
	{
		InitializeComponent();
        this.doctorService = doctorService;
    }

    private async void OnAddDoctorClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(AddDoctorPage)}");
    }
}