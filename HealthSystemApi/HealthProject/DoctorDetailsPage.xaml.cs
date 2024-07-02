using HealthProject.Models;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject;

[QueryProperty(nameof(DoctorJson), "doctorJson")]
public partial class DoctorDetailsPage : ContentPage
{
    private string doctorJson;
    public string DoctorJson
    {
        get => doctorJson;
        set
        {
            doctorJson = value;
            var hospital = JsonConvert.DeserializeObject<DoctorDetailsModel>(Uri.UnescapeDataString(value));
            BindingContext = new DoctorDetailsPageViewModel(hospital);
        }
    }
    public DoctorDetailsPage()
	{
		InitializeComponent();
	}
}