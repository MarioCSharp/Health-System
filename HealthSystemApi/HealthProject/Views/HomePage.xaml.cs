using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class HomePage : ContentPage
{
	public HomePage(HomePageViewModel homePageViewModel)
	{
		InitializeComponent();
		BindingContext = homePageViewModel;
		Title = "Клиники";
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync($"{nameof(AddHospitalPage)}");
    }
}