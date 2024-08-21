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
}