using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class DocumentViewPage : ContentPage
{
	public DocumentViewPage(DocumetnsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}