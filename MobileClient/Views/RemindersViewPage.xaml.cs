using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class RemindersViewPage : ContentPage
{
	private ReminderViewModel viewModel;
	public RemindersViewPage(ReminderViewModel viewModel)
	{
		InitializeComponent();

		Title = "Напомняния";

		BindingContext = this.viewModel = viewModel;
	}

    protected override void OnAppearing()
    {
		viewModel.LoadReminders();
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