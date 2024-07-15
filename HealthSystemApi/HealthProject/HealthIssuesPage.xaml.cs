using HealthProject.ViewModels;

namespace HealthProject;

public partial class HealthIssuesPage : ContentPage
{
	public HealthIssuesPage(HealthIssuePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}