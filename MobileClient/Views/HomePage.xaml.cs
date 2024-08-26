using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class HomePage : ContentPage
{
    private HomePageViewModel viewModel;
	public HomePage(HomePageViewModel homePageViewModel)
	{
		InitializeComponent();
		BindingContext = viewModel = homePageViewModel;
        
		Title = "�������";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.LoadHospitals();
    }
}