using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class PharmaciesViewPage : ContentPage
{
	private PharmaciesViewModel viewModel;
	public PharmaciesViewPage(PharmaciesViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = this.viewModel = viewModel;	
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}