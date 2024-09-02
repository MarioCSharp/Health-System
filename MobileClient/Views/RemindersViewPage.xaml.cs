using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class RemindersViewPage : ContentPage
{
	private ReminderViewModel viewModel;
	public RemindersViewPage(ReminderViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = this.viewModel = viewModel;
	}

    protected override void OnAppearing()
    {
		viewModel.LoadReminders();
        base.OnAppearing();
    }
}