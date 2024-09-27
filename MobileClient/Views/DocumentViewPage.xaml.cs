using HealthProject.Services.FileService;
using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class DocumentViewPage : ContentPage
{
    private DocumetnsViewModel viewModel;

    public DocumentViewPage(DocumetnsViewModel viewModel)
	{
		InitializeComponent();
        Title = "Документи";
        BindingContext = this.viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        viewModel.LoadUserDocuments();
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