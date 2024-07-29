using HealthProject.Models;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject.Views;

[QueryProperty(nameof(MedicationJson), "medicationJson")]
public partial class MedicationDetailsPage : ContentPage
{
    private string medicationJson;
    public string MedicationJson
    {
        get => medicationJson;
        set
        {
            medicationJson = value;
            var med = JsonConvert.DeserializeObject<MedicationDetailsModel>(Uri.UnescapeDataString(value));
            BindingContext = new MedicationDetailsViewModel(med);
        }
    }
    public MedicationDetailsPage()
	{
		InitializeComponent();
	}
}