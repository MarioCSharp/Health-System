using HealthProject.Models;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject;

[QueryProperty(nameof(HospitalJson), "hospitalJson")]
public partial class EditDoctorInfo : ContentPage
{
    private string hospitalJson;
    public string HospitalJson
    {
        get => hospitalJson;
        set
        {
            hospitalJson = value;
            var doctor = JsonConvert.DeserializeObject<DoctorDetailsModel>(Uri.UnescapeDataString(value));
            BindingContext = new EditDoctorInfoViewModel(doctor);
        }
    }
    public EditDoctorInfo()
	{
		InitializeComponent();
	}
}