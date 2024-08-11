using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class AppointmentHistoryPage : ContentPage
{
	private AppointmentHistoryViewModel viewModel;
	public AppointmentHistoryPage(AppointmentHistoryViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = this.viewModel = viewModel;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
		await viewModel.LoadHistory();
    }
}