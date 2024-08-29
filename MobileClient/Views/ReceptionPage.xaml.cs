using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class ReceptionPage : ContentPage
{
	public ReceptionPage(ReceptionViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}