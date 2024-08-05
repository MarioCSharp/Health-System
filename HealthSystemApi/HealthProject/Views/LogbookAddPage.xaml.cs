using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class LogbookAddPage : ContentPage
{
	public LogbookAddPage(LogbookAddViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}