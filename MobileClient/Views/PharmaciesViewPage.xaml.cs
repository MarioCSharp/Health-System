using HealthProject.Models;
using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class PharmaciesViewPage : ContentPage
{
	private PharmaciesViewModel viewModel;
	public PharmaciesViewPage(PharmaciesViewModel viewModel)
	{
		InitializeComponent();

		Title = "Аптеки";

		BindingContext = this.viewModel = viewModel;	
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.LoadPharmacies();
    }

    private async void OnPharmaciesButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(PharmaciesViewPage)}");
    }

    private async void OnLaboratoryButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(LaboratoryPage)}");
    }

    private async void OnHospitalsButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }

    private async void OnDiagnosisButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(DiagnosisPredictionPage)}");
    }
}