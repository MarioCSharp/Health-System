using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class DocumentAddPage : ContentPage
{
	public DocumentAddPage(DocumentAddViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}