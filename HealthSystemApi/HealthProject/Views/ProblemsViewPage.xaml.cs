using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class ProblemsViewPage : ContentPage
{
    public ProblemsViewPage(ProblemsViewPageViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}