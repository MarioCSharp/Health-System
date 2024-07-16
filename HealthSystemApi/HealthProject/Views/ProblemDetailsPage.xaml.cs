using HealthProject.Models;
using HealthProject.Services.ProblemService;
using HealthProject.ViewModels;
using Newtonsoft.Json;

namespace HealthProject.Views;

[QueryProperty(nameof(ProblemJson), "hospitalJson")]
public partial class ProblemDetailsPage : ContentPage
{
    private string problemJson;
    private IProblemService problemService;
    public string ProblemJson
    {
        get => problemJson;
        set
        {
            problemJson = value;
            var problem = JsonConvert.DeserializeObject<ProblemDetailsModel>(Uri.UnescapeDataString(value));
            BindingContext = new ProblemDetailsViewModel(problem, problemService);
        }
    }
    public ProblemDetailsPage(IProblemService problemService)
	{
		InitializeComponent();
        this.problemService = problemService;
	}
}