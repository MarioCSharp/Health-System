using HealthProject.ViewModels;

namespace HealthProject;

public partial class HealthIssueAddPage : ContentPage
{
	public HealthIssueAddPage(HealthIssueAddViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}