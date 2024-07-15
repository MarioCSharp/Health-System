using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel loginPageViewModel)
	{
		InitializeComponent();
		BindingContext = loginPageViewModel;
        Title = "Вписване";
    }
}