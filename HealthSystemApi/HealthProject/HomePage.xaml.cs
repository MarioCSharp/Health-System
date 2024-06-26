using HealthProject.ViewModels;

namespace HealthProject;

public partial class HomePage : ContentPage
{
	public HomePage(HomePageViewModel homePageViewModel)
	{
		InitializeComponent();
		BindingContext = homePageViewModel;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync($"//{nameof(AddHospitalPage)}");
    }
}