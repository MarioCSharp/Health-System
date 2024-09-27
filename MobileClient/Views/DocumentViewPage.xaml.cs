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