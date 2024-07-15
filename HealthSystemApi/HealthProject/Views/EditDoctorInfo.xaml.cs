using HealthProject.Models;
using HealthProject.Services.DoctorService;
using HealthProject.Services.HospitalService;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject.Views;

[QueryProperty(nameof(HospitalJson), "hospitalJson")]
public partial class EditDoctorInfo : ContentPage
{
    private string hospitalJson;
    private IDoctorService doctorService;
    private IHospitalService hospitalService;
    public string HospitalJson
    {
        get => hospitalJson;
        set
        {
            hospitalJson = value;
            var doctor = JsonConvert.DeserializeObject<DoctorDetailsModel>(Uri.UnescapeDataString(value));
            BindingContext = new EditDoctorInfoViewModel(doctor, doctorService, hospitalService);
        }
    }
    public EditDoctorInfo(IDoctorService doctorService,
                          IHospitalService hospitalService)
	{
		InitializeComponent();
        this.doctorService = doctorService;
        this.hospitalService = hospitalService;
	}
}