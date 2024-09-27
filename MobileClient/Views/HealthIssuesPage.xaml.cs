using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class HealthIssuesPage : ContentPage
{
    private HealthIssuePageViewModel viewModel;

    public HealthIssuesPage(HealthIssuePageViewModel model)
	{
		InitializeComponent();

        Title = "Здравни проблеми";

        BindingContext = viewModel = model;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.LoadHealthIssues(); 
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