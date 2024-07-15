using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class AddDoctorPage : ContentPage
{
	public AddDoctorPage(AddDoctorPageViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}