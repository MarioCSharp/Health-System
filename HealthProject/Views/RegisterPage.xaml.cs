using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterPageViewModel registerPageViewModel)
	{
		InitializeComponent();
        BindingContext = registerPageViewModel;
        Title = "Регистрация";
    }
}