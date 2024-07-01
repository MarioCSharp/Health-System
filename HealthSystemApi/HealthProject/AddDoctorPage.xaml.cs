using HealthProject.ViewModels;

namespace HealthProject;

public partial class AddDoctorPage : ContentPage
{
	public AddDoctorPage(AddDoctorPageViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}