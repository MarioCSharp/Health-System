using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class LogbookViewPage : ContentPage
{
	private LogbookPageViewModel viewModel;
	public LogbookViewPage(LogbookPageViewModel viewModel)
	{
		InitializeComponent();
        Title = "Записи";
        BindingContext = this.viewModel = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.LaodLog();
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