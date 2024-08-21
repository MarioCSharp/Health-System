using HealthProject.ViewModels;

namespace HealthProject.Views;

public partial class DocumentAddPage : ContentPage
{
	public DocumentAddPage(DocumentAddViewModel viewModel)
	{
		InitializeComponent();
        Title = "Добавяне на документ";
        BindingContext = viewModel;
	}
}