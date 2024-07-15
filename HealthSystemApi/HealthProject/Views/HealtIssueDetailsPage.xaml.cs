using HealthProject.Models;
using HealthProject.Services.HealthIssueService;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject.Views;

[QueryProperty(nameof(HealthIssueJson), "healthIssueJson")]
public partial class HealtIssueDetailsPage : ContentPage
{
    private string healthIssueJson;
    private readonly IHealthIssueService healthIssueService;
    public string HealthIssueJson
    {
        get => healthIssueJson;
        set
        {
            healthIssueJson = value;
            var hI = JsonConvert.DeserializeObject<HealthIssueDetailsModel>(Uri.UnescapeDataString(value));
            BindingContext = new HealtIssueDetailsViewModel(hI, healthIssueService);
        }
    }
    public HealtIssueDetailsPage()
	{
		InitializeComponent();
	}
}