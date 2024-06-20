namespace HealthSystem;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new HealthSystem.ViewModels.Authentication.LoginViewModel();
    }
}