using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class ReceptionPage : ContentPage
{
	private ReceptionViewModel _viewModel;
	public ReceptionPage(ReceptionViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = _viewModel = viewModel;
	}

    protected override void OnAppearing()
    {
		_viewModel.LoadHospitals();
        base.OnAppearing();
    }
}