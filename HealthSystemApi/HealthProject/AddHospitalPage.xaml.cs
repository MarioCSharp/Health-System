using HealthProject.ViewModels;

namespace HealthProject;

public partial class AddHospitalPage : ContentPage
{
	public AddHospitalPage(AddHospitalViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}