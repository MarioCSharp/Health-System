using HealthProject.Models;
using HealthProject.Services.ProblemService;
using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class ProblemAddPage : ContentPage
{
    private ProblemAddViewModel viewModel;
    private IProblemService problemService;
    public ProblemAddPage(IProblemService problemService)
	{
		InitializeComponent();

        this.problemService = problemService;
        viewModel = new ProblemAddViewModel(problemService);
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
            var selectedSymptom = e.CurrentSelection[0] as SymptomDisplayModel;
            viewModel.OnSymptomSelected(selectedSymptom);
        }
    }
}