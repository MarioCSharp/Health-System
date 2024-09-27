using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class AppointmentHistoryPage : ContentPage
{
	private AppointmentHistoryViewModel viewModel;
	public AppointmentHistoryPage(AppointmentHistoryViewModel viewModel)
	{
		InitializeComponent();
        Title = "";
        BindingContext = this.viewModel = viewModel;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
		await viewModel.LoadHistory();
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