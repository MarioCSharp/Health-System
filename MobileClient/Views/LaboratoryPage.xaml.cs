using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class LaboratoryPage : ContentPage
{
    public LaboratoryPage(LaboratoryViewModel viewModel)
    {
        InitializeComponent();

        Title = "����������� ���������";

        BindingContext = viewModel; 
    }
}