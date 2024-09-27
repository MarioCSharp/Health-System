using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class MedicationViewPage : ContentPage
{
    private MedicationPageViewModel viewModel;
	public MedicationViewPage(MedicationPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = this.viewModel = viewModel;
        Title = "���������";
	}

    protected override void OnAppearing()
    {
        viewModel.LoadMedications();
        base.OnAppearing();
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