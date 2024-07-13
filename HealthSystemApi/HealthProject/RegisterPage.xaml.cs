using HealthProject.ViewModels;

namespace HealthProject;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterPageViewModel registerPageViewModel)
	{
		InitializeComponent();
        BindingContext = registerPageViewModel;
        Title = "Регистрация";
    }
}