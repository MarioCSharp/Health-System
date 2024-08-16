using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class ProblemsViewPage : ContentPage
{
    private ProblemsViewPageViewModel viewModel;
    public ProblemsViewPage(ProblemsViewPageViewModel model)
    {
        InitializeComponent();

        Title = "Проблеми";

        BindingContext = viewModel = model;
    }

    protected override void OnAppearing()
    {
        viewModel.LoadProblems();
        base.OnAppearing();
    }
}