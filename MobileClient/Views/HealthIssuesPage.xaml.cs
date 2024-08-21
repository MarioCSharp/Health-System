using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class HealthIssuesPage : ContentPage
{
    private HealthIssuePageViewModel viewModel;

    public HealthIssuesPage(HealthIssuePageViewModel model)
	{
		InitializeComponent();

        Title = "������� ��������";

        BindingContext = viewModel = model;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.LoadHealthIssues(); 
    }
}