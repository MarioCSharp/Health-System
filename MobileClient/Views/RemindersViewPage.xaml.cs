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