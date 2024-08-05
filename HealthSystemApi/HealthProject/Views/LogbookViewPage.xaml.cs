using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class LogbookViewPage : ContentPage
{
	private LogbookPageViewModel viewModel;
	public LogbookViewPage(LogbookPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = this.viewModel = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.LaodLog();
    }
}