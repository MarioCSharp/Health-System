using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class MedicationViewPage : ContentPage
{
    private MedicationPageViewModel viewModel;
	public MedicationViewPage(MedicationPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = this.viewModel = viewModel;
	}

    protected override void OnAppearing()
    {
        viewModel.LoadMedications();
        base.OnAppearing();
    }
}