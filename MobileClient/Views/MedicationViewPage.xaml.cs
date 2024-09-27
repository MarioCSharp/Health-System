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