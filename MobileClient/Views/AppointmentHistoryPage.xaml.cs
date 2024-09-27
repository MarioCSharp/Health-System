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