using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class HealthIssueAddPage : ContentPage
{
	public HealthIssueAddPage(HealthIssueAddViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}