using HealthProject.Models;
using HealthProject.Services.AuthenticationService;
using HealthProject.Services.ProblemService;
using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class ProblemAddPage : ContentPage
{
    private ProblemAddViewModel viewModel;
    private IProblemService problemService;
    private IAuthenticationService authenticationService;
    public ProblemAddPage(IProblemService problemService,
                          IAuthenticationService authenticationService)
	{
		InitializeComponent();

        this.problemService = problemService;
        this.authenticationService = authenticationService;
        viewModel = new ProblemAddViewModel(problemService, authenticationService);
        BindingContext = viewModel;
    }

    private void OnCategorySelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            var selectedCategory = e.CurrentSelection[0] as SymptomCategoryDisplayModel;
            viewModel.OnCategorySelected(selectedCategory);
        }
    }

    private void OnSubCategorySelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            var selectedSubCategory = e.CurrentSelection[0] as SymptomSubCategoryDisplayModel;
            viewModel.OnSubCategorySelected(selectedSubCategory);
        }
    }

    private void OnSymptomSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            var selectedSymptom = e.CurrentSelection[e.CurrentSelection.Count - 1] as SymptomDisplayModel;
            viewModel.OnSymptomSelected(selectedSymptom);
        }
    }
}