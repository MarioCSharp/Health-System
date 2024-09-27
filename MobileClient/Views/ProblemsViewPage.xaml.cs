using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class ProblemsViewPage : ContentPage
{
    private ProblemsViewPageViewModel viewModel;
    public ProblemsViewPage(ProblemsViewPageViewModel model)
    {
        InitializeComponent();

        Title = "Проблеми";

        BindingContext = viewModel = model;
    }

    protected override void OnAppearing()
    {
        viewModel.LoadProblems();
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