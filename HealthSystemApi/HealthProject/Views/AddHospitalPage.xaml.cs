using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class AddHospitalPage : ContentPage
{
	public AddHospitalPage(AddHospitalViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}