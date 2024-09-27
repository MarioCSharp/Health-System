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