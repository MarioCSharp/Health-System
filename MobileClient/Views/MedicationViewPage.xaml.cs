using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class MedicationViewPage : ContentPage
{
    private MedicationPageViewModel viewModel;
	public MedicationViewPage(MedicationPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = this.viewModel = viewModel;
        Title = "Лекарства";
	}

    protected override void OnAppearing()
    {
        viewModel.LoadMedications();
        base.OnAppearing();
    }
}