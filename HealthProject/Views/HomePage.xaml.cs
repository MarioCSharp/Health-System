using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class HomePage : ContentPage
{
    private HomePageViewModel viewModel;
	public HomePage(HomePageViewModel homePageViewModel)
	{
		InitializeComponent();
		BindingContext = viewModel = homePageViewModel;
        
		Title = "Клиники";
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync($"{nameof(AddHospitalPage)}");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.LoadHospitals();
    }
}