using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class LaboratoryPage : ContentPage
{
    public LaboratoryPage(LaboratoryViewModel viewModel)
    {
        InitializeComponent();

        Title = "Лабораторни резултати";

        BindingContext = viewModel; 
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

    //private async void OnOpenCameraButtonClicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        await OpenCameraAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("Error", ex.Message, "OK");
    //    }
    //}
}