using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class HealthIssuesPage : ContentPage
{
	public HealthIssuesPage(HealthIssuePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}