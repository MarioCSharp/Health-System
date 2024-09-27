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

    private async void OnMedicineButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(MedicationViewPage)}");
    }

    private async void OnRecordsButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(LogbookViewPage)}");
    }

    private async void OnDocumentsButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(DocumentViewPage)}");
    }

    private async void OnPredictorButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(DiagnosisPredictionPage)}");
    }
}