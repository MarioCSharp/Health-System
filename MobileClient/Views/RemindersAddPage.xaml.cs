using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class RemindersAddPage : ContentPage
{
	public RemindersAddPage(ReminderAddViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}