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